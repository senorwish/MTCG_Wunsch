using MTCGServer.BLL;
using MTCGServer.BLL.Exceptions;
using MTCGServer.Core.Response;
using MTCGServer.Models;

namespace MTCGServer.API.RouteCommands.Trading
{
    public class MakeTradeCommand : AuthenticatedRouteCommand
    {
        private Guid _tradeId;
        private Guid _cardId;
        private ITradingManager _tradingManager;

        public MakeTradeCommand(ITradingManager tradingManager, User user, string tradeId, string cardId) : base(user)
        {
            _tradeId = new Guid(tradeId);
            _cardId = new Guid(cardId);
            _tradingManager = tradingManager;
        }
        public override Response Execute()
        {
            Response response = new Response();
            bool traded = false;
            try
            {
                traded = _tradingManager.MakeTrade(_tradeId, _cardId, _user);
                if (traded)
                {
                    response.StatusCode = StatusCode.Ok;
                }
                else
                {
                    response.StatusCode = StatusCode.Forbidden;
                }
            }
            catch (Exception ex)
            {
                if (ex is DataNotFoundException)
                {
                    response.StatusCode = StatusCode.NotFound;
                }
                else
                {
                    Console.WriteLine(ex.Message);
                    response.StatusCode = StatusCode.InternalServerError;
                }
            }
            return response;

        }
    }
}