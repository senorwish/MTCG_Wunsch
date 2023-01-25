using MTCGServer.BLL;
using MTCGServer.BLL.Exceptions;
using MTCGServer.Core.Request;
using MTCGServer.Core.Response;
using MTCGServer.Models;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace MTCGServer.API.RouteCommands.Cards
{
    public class GetDeckCommand : AuthenticatedRouteCommand
    {
        private readonly ICardManager _cardManager;
        private readonly bool _formatPlain;

        public GetDeckCommand(ICardManager cardManager, User user, Request request) : base(user)
        {
            _cardManager = cardManager;
            _formatPlain = GetFormatOutOfRequest(request);
        }

        private bool GetFormatOutOfRequest(Request request)
        {
            int index = request.ResourcePath.IndexOf("?format=plain");
            if (index > 0)
            {
                return true;
            }
            return false;
        }
        public override Response Execute()
        {
            Response response = new Response();
            try
            {
                List<Card> stack = _cardManager.GetDeck(_user);
                if (stack.Any())
                {
                    response.StatusCode = StatusCode.Ok;
                    if (_formatPlain)
                    {
                        string responseString = string.Empty;
                        foreach (Card card in stack)
                        {
                            responseString += $"Die Karte: {card.Id} ist {card.Name} mit {card.Damage} damage\n";
                        }
                        responseString.Remove(responseString.Length - 1);
                        response.Payload = responseString;
                    }
                    else
                    {
                        response.Payload = JsonConvert.SerializeObject(stack, Formatting.Indented);
                    }
                }
                else
                {
                    response.StatusCode = StatusCode.NoContent;
                }
            }
            catch (Exception ex)
            {
                if (ex is DataAccessException)
                {
                    response.StatusCode = StatusCode.InternalServerError;
                }
                else
                {
                    response.StatusCode = StatusCode.NotImplemented;
                }
            }
            return response;
        }
    }
}
