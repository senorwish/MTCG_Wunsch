using MTCGServer.BLL;
using MTCGServer.BLL.Exceptions;
using MTCGServer.Core.Response;
using MTCGServer.Models;

namespace MTCGServer.API.RouteCommands.Cards
{
    public class ConfigureDeckCommand : AuthenticatedRouteCommand
    {
        private readonly ICardManager _cardManager;
        private readonly List<Guid> _guids;

        public ConfigureDeckCommand(ICardManager cardManager, User user, List<Guid> guids) : base(user)
        {
            _cardManager = cardManager;
            _guids = guids;
        }

        public override Response Execute()
        {
            Response response = new Response();
            if (_guids.Count != 4)
            {
                response.StatusCode = StatusCode.BadRequest;
            }
            else
            {
                try
                {
                    bool worksFine = _cardManager.ConfigureDeck(_user, _guids);
                    if (worksFine)
                    {
                        response.StatusCode = StatusCode.Ok;
                    }
                    else
                    {
                        //Karte gehört nicht dem User oder ist nicht verfügbar
                        response.StatusCode = StatusCode.Forbidden;
                    }
                }
                catch (Exception ex)
                {
                    if (ex is UpdateFailsException) { response.StatusCode = StatusCode.InternalServerError; }
                    else if (ex is DataAccessException) { response.StatusCode = StatusCode.InternalServerError; }
                    else if (ex is DatabaseException) { response.StatusCode = StatusCode.InternalServerError; }
                    else
                    {
                        Console.WriteLine(ex.Message);
                        throw new NotImplementedException();
                    }
                }
            }
            return response;
        }
    }
}