using System.Runtime.Serialization;

namespace MTCGServer.Models
{
    public class Card
    {
        [IgnoreDataMember]
        public bool Fightable { get; set; }
        [IgnoreDataMember]
        public Elemente Element { get; set; }

        [IgnoreDataMember]
        public CardType NameEnum { get; set; }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Damage { get; set; }


        public Card(Guid id, string name, decimal damage)
        {
            Id = id;
            Name = name;
            NameEnum = Enum.Parse<CardType>(name);
            Damage = damage;
            Element = getElementFromName(NameEnum);
            Fightable = true;

        }
        public Elemente getElementFromName(CardType name)
        {
            //Feuer 
            if (name is CardType.Dragon ||
                name is CardType.Wizzard ||
                name is CardType.FireElf ||
                name is CardType.FireGoblin ||
                name is CardType.FireSpell ||
                name is CardType.FireTroll)
            {
                return Elemente.Fire;
            }
            //Wasser
            if (name == CardType.Kraken ||
                name is CardType.WaterElf ||
                name is CardType.WaterGoblin ||
                name is CardType.WaterSpell ||
                name is CardType.WaterTroll)
            {
                return Elemente.Water;
            }
            else
            {
                return Elemente.Normal;
            }


        }
    }
}
