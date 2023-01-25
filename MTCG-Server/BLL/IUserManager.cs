using MTCGServer.Models;

namespace MTCGServer.BLL
{
    public interface IUserManager
    {
        User? LoginUser(Credentials credentials);
        bool RegisterUser(Credentials credentials);
        User? GetUserByAuthToken(string authToken);
        string? GetTokenOfUsername(string username);
        UserData? GetUserData(string username);
        bool ExistUser(string username);
        bool UpdateUserData(UserData userData, string username);
    }
}
