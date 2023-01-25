using MTCGServer.Models;

namespace MTCGServer.DAL.Game
{
    public interface IGameDao
    {
        //ScoreboardData? GetIndividuelScoreboardData(User user);
        List<ScoreboardData> GetScoreboard();
        bool UpdateElo(User user);
    }
}
