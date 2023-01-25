using MTCGServer.BLL;
using MTCGServer.BLL.Exceptions;
using MTCGServer.Core.Response;
using MTCGServer.Models;

namespace MTCGServer.API.RouteCommands.Users
{
    public class UpdateUserDataCommand : AuthenticatedRouteCommand
    {
        private readonly IUserManager _userManager;
        private readonly UserData _userData;
        private readonly string _usernameToUpdateData;
        public UpdateUserDataCommand(IUserManager userManager, User user, UserData userData, string username) : base(user)
        {
            _userManager = userManager;
            _userData = userData;
            _usernameToUpdateData = username;
        }

        public override Response Execute()
        {
            var response = new Response();
            //GetTokenOfUsername wirft AccessTokenMissingException wenn Token null ist
            try
            {
                if (_userManager.ExistUser(_usernameToUpdateData))
                {
                    if (_user.Credentials.Username == "admin" || _user.Token == _userManager.GetTokenOfUsername(_usernameToUpdateData))
                    {
                        if (_userManager.UpdateUserData(_userData, _usernameToUpdateData))
                        {
                            response.StatusCode = StatusCode.Ok;
                        }
                        else
                        {
                            response.StatusCode = StatusCode.InternalServerError;
                        }
                    }
                    else
                    {
                        response.StatusCode = StatusCode.Unauthorized;
                    }
                }
                else
                {
                    //User existiert nicht
                    response.StatusCode = StatusCode.NotFound;
                }
            }
            catch (DataAccessException)
            {
                response.StatusCode = StatusCode.InternalServerError;
            }
            return response;
        }
    }
}