using MTCGServer.Core.Response;
using MTCGServer.Core.Routing;
using MTCGServer.Models;

namespace MTCGServer.API.RouteCommands
{
    public abstract class AuthenticatedRouteCommand : ICommand
    {
        protected User _user;

        public AuthenticatedRouteCommand(User identity)
        {
            _user = identity;
        }
        public abstract Response Execute();
    }
}
