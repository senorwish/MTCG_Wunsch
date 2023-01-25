using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCGServer.Models;
using NUnit.Framework;

namespace MTCGServer.Test
{
    internal class CardTest
    {
        [Test]
        public void TestCardConstructor()
        {
            // Arrange
            var id = Guid.NewGuid();
            var name = "Dragon";
            var damage = 5m;

            // Act
            var card = new Card(id, name, damage);

            // Assert
            Assert.AreEqual(id, card.Id);
            Assert.AreEqual(name, card.Name);
            Assert.AreEqual(CardType.Dragon, card.NameEnum);
            Assert.AreEqual(damage, card.Damage);
            Assert.AreEqual(Elemente.Fire, card.Element);
            Assert.IsTrue(card.Fightable);
        }

        [Test]
        public void TestGetRegualElementFromName()
        {
            // Arrange
            var card = new Card(Guid.NewGuid(), "Ork", 5m);

            // Act
            var element = card.getElementFromName(CardType.Ork);

            // Assert
            Assert.AreEqual(Elemente.Normal, element);
        }

        [Test]
        public void TestFightable()
        {
            // Arrange
            var card = new Card(Guid.NewGuid(), "Dragon", 5m);

            // Act
            card.Fightable = false;

            // Assert
            Assert.IsFalse(card.Fightable);
        }
        [Test]
        public void TestGetFireElementFromName()
        {
            // Arrange
            var card = new Card(Guid.NewGuid(), "Dragon", 5m);

            // Act
            var element = card.getElementFromName(CardType.Dragon);

            // Assert
            Assert.AreEqual(Elemente.Fire, element);
        }
        [Test]
        public void TestGetWaterElementFromName()
        {
            // Arrange
            var card = new Card(Guid.NewGuid(), "Kraken", 5m);

            // Act
            var element = card.getElementFromName(CardType.Kraken);

            // Assert
            Assert.AreEqual(Elemente.Water, element);
        }
        [Test]
        public void TestGetElementFromDragon()
        {
            // Arrange
            var card = new Card(Guid.NewGuid(), "Dragon", 5m);

            // Act
            var element = card.getElementFromName(CardType.Dragon);

            // Assert
            Assert.AreEqual(Elemente.Fire, element);
        }
        [Test]
        public void TestGetElementFromTroll()
        {
            // Arrange
            var card = new Card(Guid.NewGuid(), "FireTroll", 5m);

            // Act
            var element = card.getElementFromName(CardType.FireTroll);

            // Assert
            Assert.AreEqual(Elemente.Fire, element);
        }
        [Test]
        public void TestGetElementFromElf()
        {
            // Arrange
            var card = new Card(Guid.NewGuid(), "WaterElf", 5m);

            // Act
            var element = card.getElementFromName(CardType.WaterElf);

            // Assert
            Assert.AreEqual(Elemente.Water, element);
        }
    }
    
}
