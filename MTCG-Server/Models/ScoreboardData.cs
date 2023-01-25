namespace MTCGServer.Models
{
    public class ScoreboardData
    {
        public string? Name { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Elo { get; set; }

        public ScoreboardData()
        {
            Wins = 0;
            Losses = 0;
            Elo = 100;
        }
        public ScoreboardData(string name, int win, int loss, int elo)
        {
            Name = name;
            Elo = elo;
            Wins = win;
            Losses = loss;
        }
    }

}
