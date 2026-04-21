using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeakLogix.DAL.Interfaces;
using PeakLogix.EntityFramework.Entities.PickProSD;
using PeakLogix.EntityFramework.Contexts.Interfaces;
using PeakLogix.PickProApi.Constants;
using PeakLogix.PickProApi.Controllers.Api;
using PeakLogix.PickProApi.Startup;
using Resources.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Immutable;
using System.Data;
using AutoMapper;
using Resources.Mappers;
using System.Net;
using PeakLogix.PickProApi.Code;
using Microsoft.Extensions.Logging;
using PeakLogix.PickProApi.Models;
using Microsoft.AspNetCore.Http;
using PeakLogix.PickProApi.Services.BulkTransactions.Interfaces;
using CommunityToolkit.Diagnostics;
using PeakLogix.PickProApi.Common.Constants;
using Swashbuckle.AspNetCore.Annotations;
using PeakLogix.PickProApi.Common.DTOs;
using PeakLogix.PickProApi.Interfaces;


namespace PeakLogix.PickProApi.Controllers.Orders;

[ApiController]
[Hateoas("orders")]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = AuthConstants.BasicAuthenticationScheme)]
public class OrdersController : PaginatedControllerBase
{
    private readonly IPickProSdContext _context;
    private readonly IClaimData claims;
    private readonly IMapper _mapper;
    private readonly string _wsid;
    private readonly SdStoredProcedures _sp;
    private readonly IBulkTransactionService _bulkTransactionService;
    private readonly ILogger<OrdersController> _logger;
    private readonly IDatabaseConfig _databaseConfig;

    public OrdersController(IPickProSdContext context, IClaimData claimData, SdStoredProcedures sp, IBulkTransactionService bulkTransactionService, ILogger<OrdersController> logger, IDatabaseConfig databaseConfig)
    {
        Guard.IsNotNull(logger, nameof(logger));
        Guard.IsNotNull(context, nameof(context));
        Guard.IsNotNull(claimData, nameof(claimData));
        Guard.IsNotNull(sp, nameof(sp));
        Guard.IsNotNull(bulkTransactionService, nameof(bulkTransactionService));
        Guard.IsNotNull(databaseConfig, nameof(databaseConfig));

        _context = context;
        claims = claimData;
        _mapper = new MapperConfiguration(configuration => new OrderLineResourceMapper().ConfigureMappings(configuration)).CreateMapper();
        _wsid = claims.WSID;
        _sp = sp;
        _bulkTransactionService = bulkTransactionService;
        _logger = logger;
        _databaseConfig = databaseConfig;
    }

    [SwaggerOperation(Summary = "Gets orders for induction.", Description = "Gets orders for induction.")]
    [SwaggerResponse(200, "Orders retrieved successfully")]
    [SwaggerResponse(500, "Internal server error occurred")]
    [HttpGet("/api/orders")]
    public async Task<IActionResult> GetOrdersAsync(string type = BulkType.PICK, string status = BulkStatus.OPEN, string area = AreaType.BULK, int start = PaginationConstants.DefaultStart, int size = PaginationConstants.DefaultSize)
    {
        var orders = await _bulkTransactionService.GetOrdersForInductionAsync(type, _wsid, start, size);
        if (orders.IsSuccess)
            return Ok(orders);

        return BadRequest(orders);
    }

    [SwaggerOperation(Summary = "Gets orders count for induction.", Description = "Gets orders count for induction.")]
    [SwaggerResponse(200, "Orders count retrieved successfully")]
    [SwaggerResponse(500, "Internal server error occurred")]
    [HttpGet("count")]
    public async Task<IActionResult> GetOrdersCountAsync(string type = BulkType.PICK, string status = BulkStatus.OPEN, string area = AreaType.BULK)
    {
        var count = await _bulkTransactionService.GetOrdersCountForInductionAsync(type, _wsid);
        if (count.IsSuccess)
            return Ok(count);

        return BadRequest(count);
    }


    [HttpGet("/api/orders/{orderNumber}/OrderLines")]
    public async Task<IActionResult> GetOrdersLinesAsync(string orderNumber)
    {
        var orderLines = await _context.OpenTransactions
            .Where(openTrans => openTrans.TransactionType != TransactionType.COMPLETE && openTrans.TransactionType != TransactionType.ADJUSTMENT && openTrans.OrderNumber == orderNumber)
            .Select(ol => _mapper.Map<OrderLineResource>(ol))
            .ToListAsync();
        return Ok(orderLines);
    }
    [HttpGet("/api/orders/{orderNumber}")]
    public async Task<IActionResult> GetOrderByOrderNumberAsync(string orderNumber)
    {
        var orders = await _context.OpenTransactions
            .Where(openTrans => openTrans.OrderNumber == orderNumber)
            .Join(_context.BulkZones.Where(x => x.Wsid == _wsid),
                ot => ot.Zone,
                bz => bz.Zone,
                (ot, bulkZones) => new { openTrans = ot, BZ = bulkZones })
            .GroupBy(x => x.openTrans.OrderNumber)
            .Select(group => new OrderResource
            {
                OrderNumber = group.Key,
                ToteId = group.Min(x => x.openTrans.ToteId),
                LineCount = group.Count(),
                Priority = group.Min(x => x.openTrans.Priority),
                ImportDate = group.Min(x => x.openTrans.ImportDate),
                ImportFilename = group.Min(x => x.openTrans.ImportFilename),
                RequiredDate = group.Min(x => x.openTrans.RequiredDate),
                BatchId = group.Min(x => x.openTrans.BatchPickId),
                ExportBatchId = group.Min(x => x.openTrans.ExportBatchId),
                ToteNumber = group.Min(x => x.openTrans.ToteNumber),
                OrderLines = _mapper.Map<List<OrderLineResource>>(group.Select(x => x.openTrans).ToList())
            }).ToListAsync();

        if (orders == null)
            return StatusCode((int)HttpStatusCode.InternalServerError, HttpResponseMessage.INTERNALSERVERERROR);

        if (orders.Count() == 0)
            return NotFound();

        return Ok(orders);
    }

    [HttpPost("/api/orders/assign-tote")]
    public async Task<IActionResult> AssignToteToOrderAsync([FromBody] List<AssignToteToOrderDto> orders)
    {
        var validationResult = await ValidateOrdersInput(orders);
        if (!validationResult)
            return BadRequest();

        var duplicateCheckResult = await ValidateNoDuplicatesAsync(orders);
        if (duplicateCheckResult != null)
            return BadRequest($"Duplicate order number or tote Id provided: {duplicateCheckResult}.");

        var systemConfig = await GetSystemConfigurationAsync();
        var bulkZonesSet = await GetBulkZonesForCurrentWsidAsync();

        var toteValidationResult = await ValidateTotesNotAssignedAsync(orders);
        if (toteValidationResult != null)
            return toteValidationResult;

        var recordsToUpdate = await ProcessOrdersAsync(orders, systemConfig, bulkZonesSet);

        _context.OpenTransactions.UpdateRange(recordsToUpdate);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> ValidateOrdersInput(List<AssignToteToOrderDto> orders)
    {
        if (orders == null || orders.Count == 0)
            return false;

        return true;
    }

    private async Task<string?> ValidateNoDuplicatesAsync(List<AssignToteToOrderDto> orders)
    {
        var duplicateOrderNumber = orders.GroupBy(x => x.OrderNumber).FirstOrDefault(g => g.Count() > 1)?.Key;
        if (duplicateOrderNumber != null)
            return duplicateOrderNumber;

        var duplicateToteId = orders.GroupBy(x => x.ToteId).FirstOrDefault(g => g.Count() > 1)?.Key;
        if (duplicateToteId != null)
            return duplicateToteId;

        return null;
    }

    private async Task<(bool PickAndPass, bool MaintainBulkBatchId)> GetSystemConfigurationAsync()
    {
        var systemPreferences = await _context.SystemPreferences.FirstOrDefaultAsync();
        bool pickAndPass = systemPreferences?.PickType == PickType.PICKANDPASS;
        bool maintainBulkBatchId = systemPreferences?.BulkBatchId ?? false;
        return (pickAndPass, maintainBulkBatchId);
    }

    private async Task<HashSet<string>> GetBulkZonesForCurrentWsidAsync()
    {
        var currentBulkZones = await _context.BulkZones
            .Where(x => x.Wsid == claims.WSID)
            .Select(x => x.Zone)
            .ToListAsync();
        return currentBulkZones.ToHashSet();
    }

    private async Task<IActionResult?> ValidateTotesNotAssignedAsync(List<AssignToteToOrderDto> orderToteMap)
    {
        foreach (var orderTote in orderToteMap)
        {
            if (await _context.OpenTransactions.AnyAsync(ot => ot.ToteId == orderTote.ToteId))
                return BadRequest($"Tote id already assigned: {orderTote.ToteId}.");
        }
        return null;
    }

    private async Task<List<OpenTransaction>> ProcessOrdersAsync(
        List<AssignToteToOrderDto> orders,
        (bool PickAndPass, bool MaintainBulkBatchId) systemConfig,
        HashSet<string> bulkZonesSet)
    {
        var recordsToUpdate = new List<OpenTransaction>();
        var batchId = await GenerateBatchIdAsync();

        foreach (var order in orders)
        {
            var orderlines = await _sp.GetAvailableInductOrdersAsync(InductBy.ORDERNUMBER, order.Type, _wsid, order.OrderNumber);


            if (systemConfig.PickAndPass)
            {
                var pickAndPassRecords = await ProcessOrderInPickAndPassModeAsync(
                    order, batchId, systemConfig.MaintainBulkBatchId, bulkZonesSet);
                recordsToUpdate.AddRange(pickAndPassRecords);
            }
            else
            {
                var parallelPickRecords = await ProcessOrderInParallelPickModeAsync(
                    order, batchId);
                recordsToUpdate.AddRange(parallelPickRecords);
            }
        }

        return recordsToUpdate;
    }

    private async Task<List<OpenTransaction>> ProcessOrderInPickAndPassModeAsync(
        AssignToteToOrderDto order,
        string batchId,
        bool maintainBulkBatchId,
        HashSet<string> bulkZonesSet)
    {
        var allOrderLines = await _context.OpenTransactions
            .Where(x => x.OrderNumber == order.OrderNumber && x.TransactionType == order.Type && x.CompletedDate == null && x.ToteId ==null)
            .ToListAsync();

        var recordsToUpdate = new List<OpenTransaction>();

        foreach (var line in allOrderLines)
        {
            line.ToteId =order.ToteId;

            if (maintainBulkBatchId)
            {
                // MaintainBulkBatchId ON: all records share the same batch id across zones
                line.BatchPickId = batchId;
            }
            else
            {
                // MaintainBulkBatchId OFF: only records in current workstation's zones receive the batch id
                if (!string.IsNullOrEmpty(line.Zone) && bulkZonesSet.Contains(line.Zone))
                {
                    line.BatchPickId = batchId;
                }
            }
            recordsToUpdate.Add(line);
        }

        return recordsToUpdate;
    }

    private async Task<List<OpenTransaction>> ProcessOrderInParallelPickModeAsync(
        AssignToteToOrderDto order,
        string batchId)
    {
        var allOrderLines = await _context.OpenTransactions
            .Where(ot => 
                ot.OrderNumber == order.OrderNumber && 
                ot.TransactionType == order.Type &&
                ot.ToteId == null &&
                _context.BulkZones.Any(bz =>
                    bz.Zone == ot.Zone &&
                    bz.Wsid == _wsid))
            .ToListAsync();

        var recordsToUpdate = new List<OpenTransaction>();

        foreach (var line in allOrderLines)
        {
            // Parallel pick mode always assigns tote and batch ids to every record
            line.ToteId = order.ToteId;
            line.BatchPickId = batchId;
            recordsToUpdate.Add(line);
        }

        return recordsToUpdate;
    }

    private async Task<string> GenerateBatchIdAsync()
    {
        var batchId = await _bulkTransactionService.GetNextBatchIdAsync();
        return batchId != null && batchId.Value != null ? batchId.Value : "";
    }

    [SwaggerOperation(Summary = "Gets quick pick orders.", Description = "Gets quick pick orders.")]
    [SwaggerResponse(200, "Quick pick orders retrieved successfully")]
    [SwaggerResponse(500, "Internal server error occurred")]
    [HttpGet("quickpick")]
    public async Task<IActionResult> QuickPickOrdersAsync(int start = PaginationConstants.DefaultStart, int size = PaginationConstants.DefaultSize)
    {
        var result = await _bulkTransactionService.GetQuickPickOrdersAsync(start, size);
        
        if (result.IsSuccess)
            return Ok(result);
        
        return BadRequest(result);
    }

    [SwaggerOperation(Summary = "Gets emergency pick orders info.", Description = "Gets emergency pick orders info.")]
    [SwaggerResponse(200, "Emergency pick orders info retrieved successfully")]
    [SwaggerResponse(500, "Internal server error occurred")]
    [HttpGet("emergencypickinfo")]
    public async Task<IActionResult> GetEmergencyOrdersInfoAsync()
    {

        var result = await _bulkTransactionService.GetEmergencyOrdersInfoAsync(_wsid);

        if (result.IsSuccess)
            return Ok(result);

        return BadRequest(result);
    }

    [SwaggerOperation(Summary = "Gets emergency pick orders.", Description = "Gets emergency pick orders.")]
    [SwaggerResponse(200, "Emergency pick orders retrieved successfully")]
    [SwaggerResponse(500, "Internal server error occurred")]
    [HttpPost("emergencypick")]
    public async Task<IActionResult> EmergencyPickOrdersAsync([FromBody] PagingRequest pagingRequest)
    {

        var result = await _bulkTransactionService.GetEmergencyOrdersAsync(pagingRequest, _wsid);

        if (result.IsSuccess)
            return Ok(result);

        return BadRequest(result);
    }


    [SwaggerOperation(Summary = "Gets quick pick orders count.", Description = "Gets quick pick orders count.")]
    [SwaggerResponse(200, "Quick pick orders count retrieved successfully")]
    [SwaggerResponse(500, "Internal server error occurred")]
    [HttpGet("quickpick/count")]
    public async Task<IActionResult> GetQuickPickOrdersCountAsync()
    {
        var count = await _bulkTransactionService.GetQuickPickOrdersCountAsync();
        if (count.IsSuccess)
            return Ok(count);

        return BadRequest(count);
    }

    // API endpoint to assign orders for location assignment
    [HttpPut("/api/orders/locationassignment")]
    public async Task<IActionResult> LocationAssignmentAsync(string[] orderNumbers)
    {
        // Validate input
        if (orderNumbers == null || orderNumbers.Length == 0 || orderNumbers.Any(string.IsNullOrWhiteSpace))
            return BadRequest(HttpResponseMessage.ORDER_NUMBERS_ARE_REQUIRED);

        try
        {
            // Identify new order numbers by excluding the ones already in the database
            var ordersToAdd = (
                from orderNumber in orderNumbers
                join existing in _context.OrdersForLocAsses
                    on orderNumber equals existing.OrderNumber into gj
                from sub in gj.DefaultIfEmpty()
                where sub == null
                select new OrdersForLocAss
                {
                    OrderNumber = orderNumber,
                    TransactionType = TransactionType.PICK
                }).ToList();

            // If no new orders are found, return early with a response
            if (!ordersToAdd.Any())
                return Ok(HttpResponseMessage.ALL_ORDER_NUMBERS_ALREADY_EXISTS);

            await _context.OrdersForLocAsses.AddRangeAsync(ordersToAdd);
            await _context.SaveChangesAsync();

            return Ok(CommonResponse.CreateResponse(true));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, LogMessages.ERROR_ASSIGNING_ORDERS);
            return StatusCode(StatusCodes.Status500InternalServerError, HttpResponseMessage.INTERNALSERVERERROR);
        }
    }

    // API endpoint to check if any of the provided order numbers are already assigned for location
    [HttpPost("/api/orders/checklocationassignment")]
    public async Task<IActionResult> CheckLocationAssignmentAsync([FromBody] string[] orderNumbers)
    {
        // Validate input
        if (orderNumbers == null || orderNumbers.Length == 0 || orderNumbers.Any(string.IsNullOrWhiteSpace))
            return BadRequest(HttpResponseMessage.ORDER_NUMBERS_ARE_REQUIRED);

        try
        {
            var exists = await _context.OrdersForLocAsses
                .AnyAsync(x => orderNumbers.Contains(x.OrderNumber));

            return Ok(CommonResponse.CreateResponse(!exists));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, LogMessages.ERROR_CHECKING_LOCATION_ASSIGNMENT);
            return StatusCode(StatusCodes.Status500InternalServerError, HttpResponseMessage.INTERNALSERVERERROR);
        }
    }

    // API endpoint to check if any of the provided orders have off-carousel picks
    [HttpPost("/api/orders/checkoffcarouselpicks")]
    public async Task<IActionResult> CheckOffCarouselPicksAsync([FromBody] string[] orderNumbers)
    {
        // Validate input
        if (orderNumbers == null || orderNumbers.Length == 0 || orderNumbers.Any(string.IsNullOrWhiteSpace))
            return BadRequest(HttpResponseMessage.ORDER_NUMBERS_ARE_REQUIRED);

        try
        {
            // Check if any of the provided order numbers exist in OpenTransactions
            // AND belong to zones associated with the current user's WSID in BulkZones
            var exists = await (
                from ot in _context.OpenTransactions
                join bz in _context.BulkZones on ot.Zone equals bz.Zone
                where orderNumbers.Contains(ot.OrderNumber) && bz.Wsid == claims.WSID
                select ot
            ).AnyAsync();

            return Ok(CommonResponse.CreateResponse(!exists));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, LogMessages.ERROR_CHECKING_OFF_CAROUSEL_PICKS);
            return StatusCode(StatusCodes.Status500InternalServerError, HttpResponseMessage.INTERNALSERVERERROR);
        }
    }
}
