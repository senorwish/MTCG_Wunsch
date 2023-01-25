using MTCGServer.BLL;
using MTCGServer.BLL.Exceptions;
using MTCGServer.Core.Response;
using MTCGServer.Core.Routing;
using MTCGServer.Models;

namespace MTCGServer.API.RouteCommands.Users
{
    public class LoginCommand : ICommand
    {
        private readonly IUserManager _userManager;

        private readonly Credentials _credentials;

        public LoginCommand(IUserManager userManager, Credentials credentials)
        {
            _credentials = credentials;
            _userManager = userManager;
        }
        public Response Execute()
        {
            User? user;
            var response = new Response();
            try
            {
                user = _userManager.LoginUser(_credentials);
                if (user == null)
                {
                    response.StatusCode = StatusCode.Unauthorized;
                }
                else
                {
                    response.StatusCode = StatusCode.Ok;
                    response.Payload = user.Token;
                }
            }
            catch (DataAccessException)
            {
                response.StatusCode = StatusCode.InternalServerError;
                return response;
            }
            return response;
        }
    }
}
