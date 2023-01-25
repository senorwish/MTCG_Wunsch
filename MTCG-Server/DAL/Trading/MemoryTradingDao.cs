using MTCGServer.Models;

namespace MTCGServer.DAL.Trading
{
    public class MemoryTradingDao : ITradingDao
    {
        public bool CreateTrade(Trade trade, User user)
        {
            throw new NotImplementedException();
        }

        public bool DeleteTrade(Guid tradeId, string username)
        {
            throw new NotImplementedException();
        }

        public List<Trade> GetTrades()
        {
            /*Card? foundCard = null;

            if (player.Stack is null)
            {
                throw new TradeNotPossibleException();
            }
            //nach karte suchen
            foundCard = player.Stack.FirstOrDefault(x => x.Id == id);

            if (foundCard is null)
            {
                throw new TradeNotPossibleException();
                //user does not have this card ErrCode 403
            }
            else
            {
                if (foundCard.Fightable is false)
                {
                    throw new TradeNotPossibleException();
                }

                Id = id;
                CardToTrade = cardToTrade;
                Type = type;
                MinDamage = minDamage;

                //block foundcard to be added to the Stack
                foundCard.Fightable = false;
            }*/
            throw new NotImplementedException();
        }

        public bool MakeTrade(Guid tradeId, Guid cardId, User user)
        {
            throw new NotImplementedException();
        }
    }
}
