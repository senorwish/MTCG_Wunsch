using MTCGServer.BLL;
using MTCGServer.BLL.Exceptions;
using MTCGServer.Core.Response;
using MTCGServer.Models;

namespace MTCGServer.API.RouteCommands.Trading
{
    public class DeleteExistingTradeCommand : AuthenticatedRouteCommand
    {
        private readonly ITradingManager _tradeManager;
        private readonly Guid _tradeId;
        public DeleteExistingTradeCommand(ITradingManager tradeManager, User user, string tradeId) : base(user)
        {
            _tradeManager = tradeManager;
            _tradeId = new Guid(tradeId);
        }
        public override Response Execute()
        {
            Response response = new Response();
            try
            {
                bool deleted = _tradeManager.DeleteTrade(_user, _tradeId);
                if (deleted)
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
                if (ex is DataNotFoundException) { response.StatusCode = StatusCode.NotFound; }
                else
                {
                    response.StatusCode = StatusCode.InternalServerError;
                }
            }
            return response;
        }
    }
}