using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PeakLogix.EntityFramework.Contexts.Interfaces;
using PeakLogix.EntityFramework.Entities.PickPro_Config;
using PeakLogix.PickProApi.Controllers.Certificate;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace PeakLogix.PickProApi.Middlewares;

public class WorkstationCookieHandler(RequestDelegate next, IServiceScopeFactory scopedContextFactory, ILogger<WorkstationCookieHandler> logger)
{
    private readonly ImmutableList<string> excludedEndpoints = ExcludeFromHTTPSAttribute.GetExcludedEndpoints();
    

    public async Task InvokeAsync(HttpContext context)
    {
        // log the time it takes to handle cookie logic
        var watch = System.Diagnostics.Stopwatch.StartNew();

        // see if the current endpoint is excluded from HSTS
        string endpointName = context.Request.Path.Value ?? "";
        string controllerName = context.GetRouteData().Values["controller"]?.ToString() ?? "";
        if (excludedEndpoints.Contains(endpointName) ||
            ExcludeWsidCookieAttribute.ExcludedControllers.Contains(controllerName))
        {
            await next(context);
            return;
        }
        string? workstationId = context.Request.Cookies["WorkstationId"];
        using IServiceScope scope = scopedContextFactory.CreateScope();
        IPickProConfigContext configContext = scope.ServiceProvider.GetRequiredService<IPickProConfigContext>();
        bool validWorkstationCookie = false;
        if (workstationId != null)
        {
            var validWorkstation = await configContext.ValidWorkstations.Where(x => x.Wsid == workstationId).FirstOrDefaultAsync();
            if (validWorkstation == null || validWorkstation.PcName.Equals(workstationId))
            {
                // if they are giving us a wsid that is not in the db, 
                // or if the pc name is the same as the wsid, then we shall consider this to be invalid and we will tell the client to clear the cookie
                context.Response.Cookies.Append("WorkstationId", workstationId, new CookieOptions
                {
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    HttpOnly = false,
                    IsEssential = true,
                    Expires = System.DateTimeOffset.UtcNow.AddYears(-1) // expire the cookie
                });
            }
            else
            {
                validWorkstationCookie = true;
            }
        }
        if (!validWorkstationCookie)
        {
            // reject the request bad request

        }
        
        context.Items["WorkstationId"] = workstationId;
        watch.Stop();
        logger.LogTrace("Time to handle cookie logic: {0}ms", watch.ElapsedMilliseconds);

        await next(context);
    }

    private static async Task AddValidWorkstationAsync(IPickProConfigContext configContext, string workstationId)
    {
        ValidWorkstation newWorkstation = new();
        newWorkstation.Wsid = workstationId;
        newWorkstation.PcName = workstationId; // setting them both to the wsid means we don't know the PC name yet and we will need to force the client to enter it
        configContext.ValidWorkstations.Add(newWorkstation);
        await configContext.SaveChangesAsync();
    }
}
