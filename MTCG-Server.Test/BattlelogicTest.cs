using MTCGServer.BLL;
using MTCGServer.Models;

namespace MTCGServer.Test
{
    public class BattlelogicTest
    {
        [Test]
        public void TestFightWithSpecialRuleGoblinsVSDragon()
        {
            Credentials U1C = new Credentials("a", "b");
            UserData test = new UserData();
            ScoreboardData scoreboardData = new ScoreboardData();
            User user1 = new User(U1C, 20, test, "test", scoreboardData);
            User user2 = new User(U1C, 20, test, "test", scoreboardData);
            Battle battlefield = new Battle(user1, user2);

            MonsterCard goblin = new MonsterCard(new Guid(), "WaterGoblin", 10);
            MonsterCard dragon = new MonsterCard(new Guid(), "Dragon", 10);

            Assert.That(battlefield.SpecialRule(goblin, dragon), Is.True);
        }

        [Test]
        public void TestFightWithSpecialRuleWizzardVsOrk()
        {
            Credentials U1C = new Credentials("a", "b");
            UserData test = new UserData();
            ScoreboardData scoreboardData = new ScoreboardData();
            User user1 = new User(U1C, 20, test, "test", scoreboardData);
            User user2 = new User(U1C, 20, test, "test", scoreboardData);
            Battle battlefield = new Battle(user1, user2);

            MonsterCard goblin = new MonsterCard(new Guid(), "Wizzard", 10);
            MonsterCard dragon = new MonsterCard(new Guid(), "Ork", 10);

            Assert.That(battlefield.SpecialRule(goblin, dragon), Is.True);
        }
    }
}
