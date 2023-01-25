namespace MTCGServer.Models
{
    public class Trade
    {
        public Guid Id { get; set; }
        public Guid CardToTrade { get; set; }
        public string Type { get; set; }
        public decimal MinimumDamage { get; set; }

        public Trade(Guid id, Guid cardToTrade, string type, decimal minDamage)
        {
            Id = id;
            CardToTrade = cardToTrade;
            Type = type;
            MinimumDamage = minDamage;
        }
    }
}
