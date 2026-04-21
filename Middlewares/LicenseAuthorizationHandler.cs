using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PeakLogix.DAL.Interfaces;
using PeakLogix.NetCoreLib.LicensePoolService;
using System.Threading.Tasks;

namespace PeakLogix.PickProApi.Middlewares;
public class LicenseRequirement : IAuthorizationRequirement
{
}
public class LicenseAuthorizationHandler(
    #if !DEBUG
    ILicensePoolService licensePool, 
    #endif
    IClaimData claims, 
    ILogger<LicenseAuthorizationHandler> log, 
    IHttpContextAccessor contextAccessor) : AuthorizationHandler<LicenseRequirement>
{

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, LicenseRequirement requirement)
    {
        var workstationId = claims.WSID ?? contextAccessor.HttpContext?.Items["WorkstationId"]?.ToString();
        if (string.IsNullOrWhiteSpace(workstationId))
        {
            context.Fail();
            return;
        }
        // Using the IHttpContextAccessor to get the current HTTP context
        HttpContext? httpContext = contextAccessor.HttpContext;
        if (httpContext != null)
        {
            log.LogInformation("LicenseAuthorizationHandler invoked for endpoint {0}", httpContext.Request.Path);
            RouteData routeData = httpContext.GetRouteData();
            var controller = routeData?.Values["controller"];

            if (controller != null)
            {
                try
                {
#if !DEBUG
                    var result = await licensePool.ConsumeRefreshLicenseAsync(workstationId, controller.ToString()!);
#else
                    await Task.CompletedTask;
                    var result = true;
#endif
                    if (result)
                    {
                        context.Succeed(requirement);
                        return;
                    }
                }
                catch (System.Exception ex)
                {
                    log.LogError(ex, "Error in LicenseAuthorizationHandler");
                    var reason = new AuthorizationFailureReason(this, ex.Message);
                    context.Fail(reason);
                    return;
                }
            }
            else
            {
                // this will get hit on the licenses endpoint, which is fine because those are added dynamically
                log.LogInformation("Could not get the controller name from the route data");
                context.Succeed(requirement);
                return;
            }
        }
        else
        {
            // Just a side note, using IHttpContextAccessor should mean we won't enter this block.
            log.LogInformation("Could not get the HttpContext");
        }
    }
}
