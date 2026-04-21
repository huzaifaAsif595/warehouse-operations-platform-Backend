
namespace PeakLogix.PickProApi.Middlewares;
public interface IUserService
{
    bool ValidateTokens(string UserName, string token);
}
