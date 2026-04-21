using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PeakLogix.EntityFramework.Entities.PickProSD;
using PeakLogix.EntityFramework.Contexts.Interfaces;
using PeakLogix.PickProApi.Controllers.Api;
using PeakLogix.PickProApi.Startup;
using Resources.Resources;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PeakLogix.PickProApi.Controllers.Workstations;

[ApiController]
[Hateoas("workstations")]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = AuthConstants.BasicAuthenticationScheme)]
public class WorkstationsController : ControllerBase
{

    private readonly ILogger<WorkstationsController> log;
    private readonly IPickProSdContext context;

    public WorkstationsController(ILogger<WorkstationsController> log, IPickProSdContext context)
    {
        this.log = log;
        this.context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetWorkstationsAsync()
    {
        return Ok(await context.Workstations.ToListAsync());
    }
    [HttpPut("/api/workstations/{wsid}/bulkzone")]
    public async Task<IActionResult> UpdateBulkZoneByWorkstationsIdAsync(BulkZoneRequest request)
    {
        try
        {
            if (!string.IsNullOrEmpty(request.WSID))
            {
                var bulkzone = await context.BulkZones.Where(x => x.Wsid == request.WSID).FirstOrDefaultAsync();
                if (bulkzone != null)
                {//update 
                    bulkzone.Zone = request.Zone!;
                }
                else
                {// add new record
                    var addbulkzone = new BulkZone()
                    {
                        Wsid = request.WSID,
                        Zone = request.Zone!
                    };
                    context.BulkZones.Add(addbulkzone);
                }
                await context.SaveChangesAsync();
            }
            return Ok();
        }
        catch (Exception ex)
        {
            log.LogError(ex.Message, ex.StackTrace);
            return BadRequest(ex.Message);
        }
    }
  
}
