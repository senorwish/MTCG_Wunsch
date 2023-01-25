using MTCGServer.Models;

namespace MTCGServer.BLL
{
    public class Battle
    {
        private User _player1;
        private User _player2;
        private string _battleLock;
        public Battle(User player1, User player2)
        {
            _player1 = player1;
            _player2 = player2;
            _battleLock = "";
        }
        public string Start_Fight(ref bool updateUser)
        {
            int round = 1;
            Card card1, card2;
            Random random = new Random();
            int amount1 = 0;
            int amount2 = 0;

            while (_player1.Deck.Count > 0 && _player2.Deck.Count > 0 && round <= 100)
            {
                _battleLock += $"============== Round {round} ==============\n\n";
                amount1 = _player1.Deck.Count;
                amount2 = _player2.Deck.Count;
                card1 = _player1.Deck[random.Next(amount1)];
                card2 = _player2.Deck[random.Next(amount2)];
                if (!SpecialRule(card1, card2))
                {
                    //checkt ob Karten gleiches Element haben, returned true/false
                    if (CompareCurrentCardFighter(card1, card2))
                    {
                        //true wenn beide Monster und gleiches Element
                        SameElementFight(card1, card2);
                    }
                    else
                    {
                        //Karten haben verschiedene Elemente
                        DifferentElement(card1, card2);
                    }
                }
                round++;
            }
            if (_player1.Deck.Count is 0)
            {
                _battleLock += $"----------- {_player2.Credentials.Username} (player2) won this battle against {_player1.Credentials.Username} (player1) -----------\n";
                UpdateWinner(_player2);
                UpdateLooser(_player1);
            }
            else if (_player2.Deck.Count is 0)
            {
                _battleLock += $"----------- {_player1.Credentials.Username} (player1) won this battle against {_player2.Credentials.Username} (player2) -----------\n";
                UpdateWinner(_player1);
                UpdateLooser(_player2);
            }
            else
            {
                _battleLock += $"-------------------DRAW-------------------\n";
                updateUser = false;
            }
            return _battleLock;
        }
        public bool CompareCurrentCardFighter(Card card1, Card card2)
        {
            if (card1 is MonsterCard && card2 is MonsterCard || card1.Element == card2.Element)
                return true;

            return false;
        }
        //True falls eine Special Rule im Kampf eingesetzt wird
        public bool SpecialRule(Card card1, Card card2)
        {
            CardType[] goblins = { CardType.WaterGoblin, CardType.FireGoblin, CardType.RegularGoblin };
            //speichert Win Card
            Card? winCard = null;

            CardType[] spells = { CardType.WaterSpell, CardType.FireSpell, CardType.RegularSpell };
            //Bedigungen für Card1 um zu gewinnen
            if (card1.NameEnum is CardType.Dragon && Array.Exists(goblins, goblin => goblin == card2.NameEnum) ||
                 card1.NameEnum is CardType.Wizzard && card2.NameEnum is CardType.Ork ||
                 card1.NameEnum is CardType.WaterSpell && card2.NameEnum is CardType.Knight ||
                 card1.NameEnum is CardType.Kraken && Array.Exists(spells, x => x == card2.NameEnum) ||
                 card1.NameEnum is CardType.FireElf && card2.NameEnum is CardType.Dragon)
            {
                winCard = card1;
            }
            //Bedigungen für Card2 um zu gewinnen
            else if (Array.Exists(goblins, goblin => goblin == card1.NameEnum) && card2.NameEnum is CardType.Dragon ||
                      card1.NameEnum is CardType.Ork && card2.NameEnum is CardType.Wizzard ||
                      card1.NameEnum is CardType.Knight && card2.NameEnum is CardType.WaterSpell ||
                      Array.Exists(spells, spell => spell == card1.NameEnum) && card2.NameEnum is CardType.Kraken ||
                      card1.NameEnum is CardType.Dragon && card2.NameEnum is CardType.FireElf)
            {
                winCard = card2;
            }
            if (winCard is not null)
            {
                if (winCard == card1)
                {
                    _battleLock += $"Player1 ({_player1.Credentials.Username}) win with: {card1.Name} Damage: {card1.Damage} against\nPlayer2 ({_player2.Credentials.Username}) with: {card2.Name} Damage: {card2.Damage}\n\n";
                    _player1.Deck.Add(card2);
                    _player2.Deck.Remove(card2);
                }
                else
                {
                    _battleLock += $"Player2 ({_player2.Credentials.Username}) win with: {card2.Name} Damage: {card2.Damage} against\nPlayer1 ({_player1.Credentials.Username}) with: {card1.Name} Damage: {card1.Damage}\n\n";
                    _player2.Deck.Add(card1);
                    _player1.Deck.Remove(card1);
                }
                return true;
            }
            return false;
        }
        public void SameElementFight(Card card1, Card card2)
        {
            if (card1.Damage == card2.Damage)
            {
                _battleLock += $"........................................Draw because:........................................\nPlayer1 ({_player1.Credentials.Username}) had: {card1.Name} Damage: {card1.Damage} and\nPlayer2 ({_player2.Credentials.Username}) had {card2.Name} Damage: {card2.Damage}\n\n";
            }
            else if (card1.Damage > card2.Damage)
            {
                _battleLock += $"Player1 ({_player1.Credentials.Username}) wins round with: {card1.Name} Damage: {card1.Damage} against\nPlayer2 ({_player2.Credentials.Username}) with: {card2.Name} Damage: {card2.Damage}\n\n";
                _player1.Deck.Add(card2);
                _player2.Deck.Remove(card2);
            }
            else
            {
                _battleLock += $"Player2 ({_player2.Credentials.Username}) wins round with: {card2.Name} Damage: {card2.Damage} against\nPlayer1 ({_player1.Credentials.Username}) with: {card1.Name} Damage: {card1.Damage}\n\n";
                _player2.Deck.Add(card1);
                _player1.Deck.Remove(card1);
            }
        }
        public void DifferentElement(Card card1, Card card2)
        {
            decimal damage1 = card1.Damage;
            decimal damage2 = card2.Damage;
            //Card 1 verdoppelt damage wenn:
            if (card1.Element is Elemente.Water && card2.Element is Elemente.Fire ||
                card1.Element is Elemente.Fire && card2.Element is Elemente.Normal ||
                card1.Element is Elemente.Normal && card2.Element is Elemente.Water)
            {
                damage1 = damage1 * 2;
                damage2 = damage2 / 2;
            }
            //Card 2 verdoppelt damage wenn:
            else
            {
                damage1 = damage1 / 2;
                damage2 = damage2 * 2;
            }

            if (card1.Damage == card2.Damage)
            {
                _battleLock += $"........................................Draw because:........................................\nPlayer1 ({_player1.Credentials.Username}) had: {card1.Name} Damage: {card1.Damage} => Damage: {damage1} and\nPlayer2 ({_player2.Credentials.Username}) had {card2.Name} Damage: {card2.Damage} => Damage: {damage1}\n\n";
            }
            else if (damage1 > damage2)
            {
                _battleLock += $"Player1 ({_player1.Credentials.Username}) wins round with: {card1.Name} Damage: {card1.Damage} => calculated Damage: {damage1} against\nPlayer2 ({_player2.Credentials.Username}): {card2.Name} Damage: {card2.Damage} => Damage: {damage2}\n\n";
                _player1.Deck.Add(card2);
                _player2.Deck.Remove(card2);
            }
            else
            {
                _battleLock += $"Player2 ({_player2.Credentials.Username}) win round with: {card2.Name} Damage: {card2.Damage} => calculated Damage: {damage2} against\nPlayer1 ({_player1.Credentials.Username}):{card1.Name} Damage: {card1.Damage} => Damage: {damage1}\n\n";
                _player2.Deck.Add(card1);
                _player1.Deck.Remove(card1);
            }
        }
        void UpdateWinner(User winner)
        {
            winner.ScoreboardData.Wins += 1;
            
            if(winner.ScoreboardData.Wins % 5 == 0)
            {
                winner.ScoreboardData.Elo += 3;
            }
            winner.ScoreboardData.Elo += 3;
        }
        void UpdateLooser(User looser)
        {
            looser.ScoreboardData.Losses += 1;
            looser.ScoreboardData.Elo -= 5;
        }
    }
}
