using MTCGServer.BLL;
using MTCGServer.Core.Request;
using MTCGServer.Models;

namespace MTCGServer.API.RouteCommands
{
    public class IdentityProvider : IIdentityProvider
    {
        private readonly IUserManager _userManager;

        public IdentityProvider(IUserManager userManager)
        {
            _userManager = userManager;
        }
        public User? GetIdentityForRequest(Request request)
        {
            User? currentUser = null;
            if (request.Header.TryGetValue("Authorization", out var authToken))
            {
                const string prefix = "Bearer ";
                if (authToken.StartsWith(prefix))
                {
                    currentUser = _userManager.GetUserByAuthToken(authToken.Substring(prefix.Length));
                }
            }
            return currentUser;
        }
    }
}
