namespace MTCGServer.Models
{
    public class SpellCard : Card
    {
        public SpellCard(Guid id, string name, decimal damage) : base(id, name, damage) { }
    }
}
