
using MTCGServer.BLL;
using MTCGServer.BLL.Exceptions;
using MTCGServer.Core.Response;
using MTCGServer.Core.Routing;
using MTCGServer.Models;

namespace MTCGServer.API.RouteCommands.Users
{
    public class RegisterCommand : ICommand
    {
        private readonly Credentials _credentials;
        private readonly IUserManager _userManager;

        public RegisterCommand(IUserManager userManager, Credentials credentials)
        {
            _credentials = credentials;
            _userManager = userManager;
        }

        public Response Execute()
        {
            var response = new Response();
            try
            {
                if (_userManager.RegisterUser(_credentials))
                {
                    response.StatusCode = StatusCode.Created;
                }
            }
            catch (Exception ex)
            {
                if (ex is DuplicateDataException)
                {
                    response.StatusCode = StatusCode.Conflict;
                }
                if (ex is DataAccessException)
                {
                    response.StatusCode = StatusCode.InternalServerError;
                }
            }
            return response;
        }
    }
}
