using MTCGServer.Models;

namespace MTCGServer.DAL.Users
{
    public interface IUserDao
    {
        User? GetUserByAuthToken(string authToken);
        User? GetUserByCredentials(string username, string password);
        bool InsertUser(User user);
        string? GetToken(string username);
        UserData? GetUserData(string username);
        bool ExistUser(string username);
        bool UpdateUserData(UserData userData, string username);
    }
}
