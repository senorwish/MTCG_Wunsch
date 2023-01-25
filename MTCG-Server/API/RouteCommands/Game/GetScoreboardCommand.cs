using MTCGServer.BLL;
using MTCGServer.BLL.Exceptions;
using MTCGServer.Core.Response;
using MTCGServer.Core.Routing;
using MTCGServer.Models;
using Newtonsoft.Json;

namespace MTCGServer.API.RouteCommands
{
    public class GetScoreboardCommand : ICommand
    {
        private IGameManager _gameManager;

        public GetScoreboardCommand(IGameManager gameManager)
        {
            _gameManager = gameManager;
        }
        public Response Execute()
        {
            Response response = new Response();
            List<ScoreboardData> scoreboard = new List<ScoreboardData>();
            try
            {
                scoreboard = _gameManager.GetScoreboard();
                if (scoreboard.Any())
                {
                    response.StatusCode = StatusCode.Ok;
                    response.Payload = JsonConvert.SerializeObject(scoreboard, Formatting.Indented);
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