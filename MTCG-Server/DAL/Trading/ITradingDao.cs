using MTCGServer.Models;

namespace MTCGServer.DAL.Trading
{
    public interface ITradingDao
    {
        bool CreateTrade(Trade trade, User user);
        bool DeleteTrade(Guid tradeId, string username);
        List<Trade> GetTrades();
        bool MakeTrade(Guid tradeId, Guid cardId, User user);
    }
}
