using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeakLogix.EntityFramework.Entities.PickProSD;
using PeakLogix.EntityFramework.Contexts.Interfaces;
using PeakLogix.PickProApi.Controllers.Api;
using PeakLogix.PickProApi.Startup;
using System.Linq;
using System.Threading.Tasks;
namespace PeakLogix.PickProApi.Controllers.SystemPreferences;

[Authorize(AuthenticationSchemes = AuthConstants.BasicAuthenticationScheme)]
//TODO Add:[Authorize(Policy = AuthConstants.LicenseAuthorizationPolicy)]
[Route("api/[controller]")]
[ApiController]
[Hateoas("systempreferences")]
public class SystemPreferencesController(IPickProSdContext sdContext) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetSystemPreferencesAsync()
    {
        
        return Ok(new SystemPreference((await sdContext.SystemPreferences.FirstOrDefaultAsync())!) { CompanyLogo = null });
    }

}
