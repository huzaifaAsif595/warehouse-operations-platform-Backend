using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using PeakLogix.PickProApi.Collections;
using PeakLogix.PickProApi.Models;
using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace PeakLogix.PickProApi.Middlewares;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    readonly IUserCollection userCollection;
    public BasicAuthenticationHandler(IUserCollection userCollection,
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : base(options, logger, encoder)
    {
        this.userCollection = userCollection;
    }


    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {

        // log the time it takes to authenticate
        var watch = System.Diagnostics.Stopwatch.StartNew();

        // first we need to figure out if this is a config user or a regular user
        bool isConfigUser = IsConfigUser(Request);
        var jwtPayload = GetJwtPayload(Request, isConfigUser);
        Claim[] claims;
        if (isConfigUser)
        {
            if(!userCollection.IsValidConfigUser(jwtPayload.UserName!, jwtPayload.Token!))
            {
                return AuthenticateResult.Fail("Invalid Token");
            }
            claims = [new Claim(ClaimTypes.Name, jwtPayload.UserName!)];
        }
        else
        {

            if (!userCollection.IsValidUser(jwtPayload.UserName!, jwtPayload.Token!))
            {
                return AuthenticateResult.Fail("Invalid Token");
            }
            string workstationId = Request.HttpContext.Items["WorkstationId"]!.ToString()!;
            claims = [new Claim(ClaimTypes.Name, jwtPayload.UserName!), new Claim(ClaimTypes.Sid, workstationId)];
        }


        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);
        await Task.CompletedTask;
        var result = AuthenticateResult.Success(ticket);
        watch.Stop();
        var elapsedMs = watch.ElapsedMilliseconds;
        Logger.LogTrace($"Time to authenticate: {elapsedMs}ms");
        return result;

        
    }

    private JwtPayload GetJwtPayload(HttpRequest request, bool isConfigUser)
    {
        if (isConfigUser)
        {
            var token = UtilityHelper.decodeJWT(Request.Cookies["jwt"]!)!;
            token.Token = Request.Cookies["jwt"]!;
            return token;
        }
        else
        {
            var authHeader = AuthenticationHeaderValue.Parse(request.Headers["_token"]!);
            var token = UtilityHelper.decodeJWT(authHeader.ToString())!;
            token.Token = authHeader.ToString();
            return token;
        }
    }

    private static bool IsConfigUser(HttpRequest request)
    {
        var jwtToken = request.Headers["_token"].ToString();
        if (string.IsNullOrEmpty(jwtToken))
        {
            // it might be in the cookie "jwt"
            jwtToken = request.Cookies["jwt"]!;
            var configToken = UtilityHelper.decodeJWT(jwtToken);
            if (configToken == null)
            {
                throw new ArgumentException("Invalid Token");
            }
            else
            {
                return true;
            }

        }
        return false;
    }
}
