using MTCGServer.Core.Request;
using MTCGServer.Models;

namespace MTCGServer.API.RouteCommands
{
    public interface IIdentityProvider
    {
        public User? GetIdentityForRequest(Request request);
    }
}
