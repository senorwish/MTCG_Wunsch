using MTCGServer.BLL;
using MTCGServer.BLL.Exceptions;
using MTCGServer.Core.Response;
using MTCGServer.Models;
using Newtonsoft.Json;

namespace MTCGServer.API.RouteCommands.Trading
{
    public class GetTradesCommand : AuthenticatedRouteCommand
    {
        private ITradingManager _tradingManager;
        public GetTradesCommand(ITradingManager tradingManager, User user) : base(user)
        {
            _tradingManager = tradingManager;
        }
        public override Response Execute()
        {
            Response response = new Response();
            try
            {
                List<Trade> trades = _tradingManager.GetTrades();
                if (trades.Any())
                {
                    response.StatusCode = StatusCode.Ok;
                    response.Payload = JsonConvert.SerializeObject(trades, Formatting.Indented);
                }
                else
                {
                    response.StatusCode = StatusCode.NoContent;
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