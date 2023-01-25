namespace MTCGServer.Models
{
    public class MonsterCard : Card
    {
        public MonsterCard(Guid id, string name, decimal damage) : base(id, name, damage) { }
    }
}
