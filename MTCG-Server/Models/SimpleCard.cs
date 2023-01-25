namespace MTCGServer.Models
{
    public class SimpleCard
    {
        public string Name;
        public string Damage;
        public string Id;

        public SimpleCard(string id, string name, string damage)
        {
            Id = id;
            Name = name;
            Damage = damage;
        }
    }

}

