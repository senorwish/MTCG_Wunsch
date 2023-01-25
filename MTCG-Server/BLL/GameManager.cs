using MTCGServer.BLL.Exceptions;
using MTCGServer.DAL.Exceptions;
using MTCGServer.DAL.Game;
using MTCGServer.Models;

namespace MTCGServer.BLL
{
    public class GameManager : IGameManager
    {
        private readonly IGameDao _gameDao;
        public GameManager(IGameDao gameDao)
        {
            _gameDao = gameDao;
        }
        public List<ScoreboardData> GetScoreboard()
        {
            try
            {
                return _gameDao.GetScoreboard();
            }
            catch (Exception ex)
            {
                if (ex is DataAccessFailedException)
                {
                    throw new DataAccessException();
                }
                else
                {
                    throw;
                }
            }
        }
        public bool UpdateElo(User user)
        {
            try
            {
                return _gameDao.UpdateElo(user);
            }
            catch (Exception ex)
            {
                if (ex is DataAccessFailedException) { throw new DataAccessException(); }
                else { throw; }
            }
        }
    }
}
