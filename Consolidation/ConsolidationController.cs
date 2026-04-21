using CommunityToolkit.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using PeakLogix.PickProApi.Controllers.Api;
using PeakLogix.PickProApi.Common.DTOs;
using PeakLogix.PickProApi.Common.DTOs.Consolidation;
using PeakLogix.PickProApi.Services.Consolidation.Interfaces;
using PeakLogix.PickProApi.Startup;
using Resources.Resources.Consolidation;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Immutable;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace PeakLogix.PickProApi.Controllers.Consolidation
{
    [ApiController]
    [Hateoas("consolidation")]
    [Route("api/[Controller]")]
    [Authorize(AuthenticationSchemes = AuthConstants.BasicAuthenticationScheme)]
    public class ConsolidationController : ControllerBase
    {
        private readonly ILogger<ConsolidationController> _logger;
        private readonly IConsolidationService _consolidationService;
        private readonly LinkGenerator _linkGenerator;

        public ConsolidationController(
            ILogger<ConsolidationController> logger,
            IConsolidationService consolidationService,
            LinkGenerator linkGenerator)
        {
            Guard.IsNotNull(logger);
            Guard.IsNotNull(consolidationService);
            Guard.IsNotNull(linkGenerator);

            _logger = logger;
            _consolidationService = consolidationService;
            _linkGenerator = linkGenerator;
        }

        [SwaggerOperation(Summary = "Gets all consolidation zones", Description = "Retrieves a list of all consolidation zones with pagination.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success - Returns list of consolidation zones")]
        [HttpGet("Zones")]
        public async Task<IActionResult> GetConsolidationZonesAsync()
        {
            var pagingRequest = new PagingRequest();

            var result = await _consolidationService.GetConsolidationZonesAsync(pagingRequest);

            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(result.pagingInfo));

            var zonesWithLinks = result.consolidationZones.ConsolidationZones.Select(x =>
                new LinkedResource<ConsolidationZone>(x, CreateLinksForConsolidationZone(x.ConsolidationZoneID)));

            return Ok(zonesWithLinks);
        }

        [SwaggerOperation(Summary = "Gets consolidation zone status counts", Description = "Retrieves status counts for a specific consolidation zone.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success - Returns consolidation status counts")]
        [HttpGet("ZoneStatus/{consolidationZone}", Name = "GetConsolidationZoneStatusAsync")]
        public async Task<IActionResult> GetConsolidationZoneStatusAsync(string consolidationZone)
        {
            var result = await _consolidationService.GetConsolidationStatusCountsAsync(consolidationZone);

            return Ok(result);
        }

        [SwaggerOperation(Summary = "Gets route status counts for a zone", Description = "Retrieves route ID status counts for a specific consolidation zone.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success - Returns route status counts")]
        [HttpGet("Zone/{consolidationZone}/RoutesStatus", Name = "GetConsolidationZoneRoutesStatusAsync")]
        public async Task<IActionResult> GetConsolidationZoneRoutesStatusAsync(string consolidationZone)
        {
            var result = await _consolidationService.GetRouteIdStatusAsync(consolidationZone);

            return Ok(result);
        }

        [SwaggerOperation(Summary = "Gets consolidation routes for a zone", Description = "Retrieves paginated list of consolidation routes for a specific zone with search and sort capabilities.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success - Returns list of consolidation routes")]
        [HttpGet("Routes/{consolidationZone}", Name = "GetConsolidationRoutesAsync")]
        public async Task<IActionResult> GetConsolidationRoutesAsync(
            string consolidationZone,
            [FromQuery] PagingRequest pageParams,
            [FromQuery] ConsolidationZoneRouteSearchParams searchAndSortParams)
        {
            searchAndSortParams.PagingRequest = pageParams;

            var (zoneRoutes, pagingInfo) = await _consolidationService.GetConsolidationZoneRoutesAsync(
                consolidationZone,
                searchAndSortParams);

            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(pagingInfo));

            var linkedRoutes = zoneRoutes.ConsolidationZoneRoutes
                .Select(route => new LinkedResource<ConsolidationZoneRoute>(route, CreateLinksForRoutes(route.RouteID)));

            return Ok(linkedRoutes);
        }

        [SwaggerOperation(Summary = "Gets route details", Description = "Retrieves detailed information for a specific consolidation route including orders.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success - Returns route details")]
        [HttpGet("Route/{routeId}", Name = "GetConsolidationRouteDetailsAsync")]
        public async Task<IActionResult> GetConsolidationRouteDetailsAsync(string routeId)
        {
            var routeDetails = await _consolidationService.GetRouteDetailsAsync(routeId);

            return Ok(routeDetails);
        }

        [SwaggerOperation(Summary = "Updates zone auto release thresholds", Description = "Updates the upper and lower auto release thresholds for a consolidation zone.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success - Thresholds updated")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Failed to update thresholds")]
        [HttpPut("RouteThresholds/{consolidationZone}", Name = "PutConsolidationZoneThresholdsAsync")]
        public async Task<IActionResult> PutConsolidationZoneThresholdsAsync(
            string consolidationZone,
            [FromBody] ConsolidationZoneThresholdRequest thresholdRequest)
        {
            var updateResult = await _consolidationService.UpdateZoneAutoReleaseThresholdsAsync(
                consolidationZone,
                thresholdRequest.UpperThreshold,
                thresholdRequest.LowerThreshold);

            if (updateResult)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [SwaggerOperation(Summary = "Requests release for a route", Description = "Requests release for a consolidation route via external STE API.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success - Release requested")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Failed to request release")]
        [HttpPatch("Route/{routeId}/RequestRelease", Name = "RequestRouteReleaseAsync")]
        public async Task<IActionResult> RequestRouteReleaseAsync(string routeId)
        {
            var result = await _consolidationService.RequestReleaseAsync(routeId);

            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [SwaggerOperation(Summary = "Updates consolidation zone status", Description = "Updates the status of a consolidation zone via external STE API.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success - Zone status updated")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Failed to update zone status")]
        [HttpPatch("ConZone/Status", Name = "UpdateConZoneStatusAsync")]
        public async Task<IActionResult> UpdateConZoneStatusAsync([FromBody] ConsolidationZoneStatusRequest statusRequest)
        {
            var result = await _consolidationService.UpdateConZoneStatusAsync(statusRequest.ConZone, statusRequest.Status.Key);

            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        private ImmutableList<Link> CreateLinksForConsolidationZone(string consolidationZoneId)
        {
            var links = ImmutableList.CreateRange(
            [
                new Link("ConsolidationRoutes", _linkGenerator.GetPathByRouteValues(routeName: "GetConsolidationRoutesAsync",
                values: new { consolidationZone = consolidationZoneId, SelectedPage = 1, PageSize = 10 })!),

                new Link("ConsolidationZoneStatus", _linkGenerator.GetPathByRouteValues(routeName: "GetConsolidationZoneStatusAsync",
                values: new { consolidationZone = consolidationZoneId })!),

                new Link("ConsolidationZoneRoutesStatus",
                _linkGenerator.GetPathByRouteValues(routeName: "GetConsolidationZoneRoutesStatusAsync",
                values: new { consolidationZone = consolidationZoneId })!),

                new Link("ConsolidationRouteThresholds", _linkGenerator.GetPathByRouteValues(routeName: "PutConsolidationZoneThresholdsAsync",
                values: new { consolidationZone = consolidationZoneId })!,
                "PUT"),
            ]);

            return links;
        }

        private ImmutableList<Link> CreateLinksForRoutes(string routeId)
        {
            var links = ImmutableList.CreateRange(
           [
               new Link("ConsolidationRouteDetails", _linkGenerator.GetPathByRouteValues(routeName: "GetConsolidationRouteDetailsAsync",
                values: new { routeId = routeId })!),
                new Link("RequestRelease", _linkGenerator.GetPathByRouteValues(routeName: "RequestRouteReleaseAsync",
                values: new { routeId = routeId })!)
            ]);

            return links;
        }
    }
}
