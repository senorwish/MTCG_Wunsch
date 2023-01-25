using MTCGServer.Models;

namespace MTCGServer.BLL
{
    public interface IGameManager
    {
        List<ScoreboardData> GetScoreboard();
        bool UpdateElo(User user);
    }
}
