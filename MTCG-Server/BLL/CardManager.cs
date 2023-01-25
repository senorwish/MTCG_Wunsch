using MTCGServer.BLL.Exceptions;
using MTCGServer.DAL;
using MTCGServer.DAL.Cards;
using MTCGServer.DAL.Exceptions;
using MTCGServer.Models;
using Npgsql;

namespace MTCGServer.BLL
{
    public class CardManager : ICardManager
    {
        private readonly ICardDao _cardDao;

        public CardManager(ICardDao cardDao)
        {
            _cardDao = cardDao;
        }
        public bool ConfigureDeck(User user, List<Guid> guids)
        {
            try
            {
                return _cardDao.ConfigureDeck(user, guids);
            }
            catch (Exception ex)
            {
                if (ex is DataUpdateException) { throw new UpdateFailsException(); }
                else if (ex is DataAccessFailedException) { throw new DataAccessException(); }
                else if (ex is NpgsqlException) { throw new DatabaseException(); }
                else { throw; }
            }
        }
        public List<Card> GetDeck(User user)
        {
            try
            {
                return _cardDao.GetDeck(user);
            }
            catch (DataAccessFailedException)
            {
                throw new DataAccessException();
            }
        }
        public List<Card> GetStack(User user)
        {
            try
            {
                return _cardDao.GetStack(user);
            }
            catch (DataAccessFailedException)
            {
                throw new DataAccessException();
            }
        }
    }
}
