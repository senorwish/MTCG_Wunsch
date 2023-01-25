using MTCGServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace MTCGServer.Test
{
    internal class TestUser
    {
        [Test]
        public void TestNewUserMoney()
        {
            // Arrange Act
            var user = new User(new Credentials("Name", "Passwort"));

            // Assert
            Assert.AreEqual(user.Money, 20);
        }
        [Test]
        public void TestEloForNewUser()
        {
            // Arrange Act
            var user = new User(new Credentials("Name", "Passwort"));

            // Assert
            Assert.AreEqual(user.ScoreboardData.Elo, 100);
        }
        [Test]
        public void TestWinForNewUser()
        {
            // Arrange Act
            var user = new User(new Credentials("Name", "Passwort"));

            // Assert
            Assert.AreEqual(user.ScoreboardData.Wins, 0);
        }
        [Test]
        public void TestLoseForNewUser()
        {
            // Arrange Act
            var user = new User(new Credentials("Name", "Passwort"));

            // Assert
            Assert.AreEqual(user.ScoreboardData.Losses, 0);
        }
    }

}
