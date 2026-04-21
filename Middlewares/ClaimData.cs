using Microsoft.AspNetCore.Http;
using PeakLogix.DAL.Interfaces;
using PeakLogix.PickProApi.Models;
using System.Security.Claims;

namespace PeakLogix.PickProApi.Middlewares;


public class ClaimData : IClaimData
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public ClaimData(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public string UserName => _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name)!;
    public string WSID => _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Sid)!;
    public ClaimObject GetClaims()
    {
        var claimObject = new ClaimObject();
        claimObject.UserName = UserName;
        claimObject.WSID = WSID;
        return claimObject;
    }
}
