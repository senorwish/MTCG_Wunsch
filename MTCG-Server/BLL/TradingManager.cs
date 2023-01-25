using MTCGServer.BLL.Exceptions;
using MTCGServer.DAL;
using MTCGServer.DAL.Exceptions;
using MTCGServer.DAL.Trading;
using MTCGServer.Models;
using Npgsql;

namespace MTCGServer.BLL
{
    public class TradingManager : ITradingManager
    {
        private ITradingDao _tradeDao;

        public TradingManager(ITradingDao tradeDao)
        {
            _tradeDao = tradeDao;
        }
        public bool CreateTrade(Trade trade, User user)
        {
            try
            {
                return _tradeDao.CreateTrade(trade, user);
            }
            catch (Exception ex)
            {
                if (ex is DataAccessFailedException)
                {
                    throw new DataAccessException();
                }
                else if (ex is PostgresException)
                {
                    throw new DuplicateDataException();
                }
                else
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
        }
        public bool DeleteTrade(User user, Guid tradeId)
        {
            try
            {
                return _tradeDao.DeleteTrade(tradeId, user.Credentials.Username);
            }
            catch (Exception ex)
            {
                if (ex is DataDeletingFailsException)
                {
                    throw new DeletingFailsException();
                }
                else if (ex is DataAccessFailedException) { throw new DataAccessException(); }
                else if (ex is TradeIdNotFoundException) { throw new DataNotFoundException(); }
                else { throw; }
            }
        }

        public List<Trade> GetTrades()
        {
            try
            {
                return _tradeDao.GetTrades();
            }
            catch (DataAccessFailedException)
            {
                throw new DataAccessException();
            }
        }

        public bool MakeTrade(Guid tradeId, Guid cardId, User user)
        {
            try
            {
                return _tradeDao.MakeTrade(tradeId, cardId, user);
            }
            catch (Exception ex)
            {
                if (ex is TradeIdNotFoundException) { throw new DataNotFoundException(); }
                if (ex is DataAccessFailedException) { throw new DataAccessException(); }
                if (ex is DataDeletingFailsException) { throw new DeletingFailsException(); }
                if (ex is DataUpdateException) { throw new UpdateFailsException(); }
                else { throw; }
            }

        }
    }
}
