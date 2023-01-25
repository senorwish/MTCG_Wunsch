namespace MTCGServer.Models
{
    public class Store
    {
        public List<Trade> _currentTrades;
        private List<Package> _currentPackages;

        public Store()
        {
            _currentPackages = new List<Package>();
            _currentTrades = new List<Trade>();
        }
        public void listTrades()
        {
            if (_currentTrades == null)
            {
            }
            else
            {
                int count = 1;
                foreach (Trade trade in _currentTrades)
                {
                    Console.WriteLine($"Trade {count}:\nTrade Id: {trade.Id}\nCard Id: {trade.Id}\n wanted Card type: {trade.Type}\nminimum Damage: {trade.MinimumDamage} ");
                    count++;
                }
            }

        }
        public void addTrade(Trade trade)
        {
            Trade? existingTrade = null;
            //Check ob Trade exisitiert
            existingTrade = _currentTrades.FirstOrDefault(x => x.Id == trade.Id);

            if (existingTrade is null)
            {
                _currentTrades.Add(trade);
                //Successful
            }
            else
            {
                //Trade existiert bereits
            }
        }
        public void deleteTrade(Guid tradeId, User player)
        {
            //search for this trade
            Trade? foundTrade = null;
            foundTrade = _currentTrades.FirstOrDefault(x => x.Id == tradeId);

            if (foundTrade is null)
            {
            }
            //Checkt ob Karte im Trade auch dem User gehört
            else
            {
                Guid id = foundTrade.CardToTrade;
                Card? foundCard = null;
                foundCard = player.Stack.FirstOrDefault(x => x.Id == id);
                if (foundCard is null)
                {
                    //Karte gehört nicht dem User
                }
                else
                {
                    _currentTrades.Remove(foundTrade);
                    //Successful
                }
            }
        }

        public void buyPackage(User player)
        {
            if (_currentPackages is null)
            {

            }
            else
            {
                Package package = _currentPackages.First();
                if (player.Money >= package.Price)
                {
                    if (player.Money >= package.Price)
                    {
                        _currentPackages.RemoveAt(0);
                        foreach (Card card in package.PackageOfCards)
                        {
                            player.Stack.Add(card);
                        }
                        //Success
                    }
                }
                else
                {
                    //Zu wenig Geld für Trade
                }
            }
        }
    }
}
