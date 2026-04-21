using PeakLogix.PickProApi.Collections;

namespace PeakLogix.PickProApi.Middlewares;

public class UserService(IUserCollection userCollection) : IUserService
{

    private readonly IUserCollection userCollection = userCollection;


    public bool ValidateTokens(string username, string token)
    {
        return userCollection.IsValidUser(username, token);
    }
}
