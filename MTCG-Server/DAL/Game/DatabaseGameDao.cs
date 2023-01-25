using MTCGServer.DAL.Exceptions;
using MTCGServer.Models;
using Npgsql;
using SWE1.MessageServer.DAL;
using System.Data;
using System.Text;

namespace MTCGServer.DAL.Game
{
    public class DatabaseGameDao : DatabaseDao, IGameDao
    {
        public DatabaseGameDao(string connectionString) : base(connectionString) { }

        public List<ScoreboardData> GetScoreboard()
        {
            List<ScoreboardData> scoreboard = new List<ScoreboardData>();
            try
            {
                return ExecuteWithDbConnection((connection) =>
                {
                    //Create the list of ScoreboardData in descending order
                    string query = "SELECT name, elo, wins, losses FROM users ORDER BY elo DESC";
                    using NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
                    cmd.Prepare();
                    using NpgsqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        //bereits sortiert
                        scoreboard.Add(new ScoreboardData(reader.GetString("name"), reader.GetInt16("wins"), reader.GetInt16("losses"), reader.GetInt16("elo")));
                    }
                    return scoreboard;
                });
            }
            catch (Exception ex)
            {
                if (ex is DataAccessFailedException)
                {
                    throw;
                }
                else
                {
                    throw;
                }
            }
        }

        public bool UpdateElo(User user)
        {
            try
            {
                return ExecuteWithDbConnection((connection) =>
                {
                    string query = "UPDATE users SET elo = @elo, wins = @wins, losses = @losses WHERE username = @username";
                    using NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("elo", user.ScoreboardData.Elo);
                    cmd.Parameters.AddWithValue("wins", user.ScoreboardData.Wins);
                    cmd.Parameters.AddWithValue("losses", user.ScoreboardData.Losses);
                    cmd.Parameters.AddWithValue("username", user.Credentials.Username);
                    cmd.Prepare();

                    if (cmd.ExecuteNonQuery() != 1)
                    {
                        return false;
                    }
                    return true;
                });
            }
            catch (Exception ex)
            {
                if (ex is DataAccessFailedException)
                {
                    throw;
                }
                else
                {
                    throw;
                }
            }
        }

        /*public ScoreboardData? GetIndividuelScoreboardData(User user)
        {
            ScoreboardData? stats = null;
            try
            {
                return ExecuteWithDbConnection((connection) =>
                {
                    string query = "SELECT name, elo, wins, losses FROM users WHERE username = @username";
                    using NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("username", user.Credentials.Username);
                    cmd.Prepare();
                    using NpgsqlDataReader reader = cmd.ExecuteReader();
                    if(reader.Read())
                    {
                        stats = new ScoreboardData(reader.GetString("name"), reader.GetInt16("elo"), reader.GetInt16("wins"), reader.GetInt16("losses"));
                    }
                    return stats;
                });
            }
            catch (Exception ex)
            {
                if (ex is DataAccessFailedException)
                {
                    throw;
                }
                else
                {
                    throw;
                }
            }
        }*/
    }
}
