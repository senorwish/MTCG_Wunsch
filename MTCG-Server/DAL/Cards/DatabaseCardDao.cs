using MTCGServer.DAL.Exceptions;
using MTCGServer.Models;
using Npgsql;
using SWE1.MessageServer.DAL;
using System.Data;
using System.Text;

namespace MTCGServer.DAL.Cards
{
    public class DatabaseCardDao : DatabaseDao, ICardDao
    {
        public DatabaseCardDao(string connectionString) : base(connectionString) { }

        public bool ConfigureDeck(User user, List<Guid> guids)
        {
            bool possibleToAdd = true;
            bool updatingWorks = true;
            try
            {
                foreach (Guid id in guids)
                {
                    possibleToAdd = ExecuteWithDbConnection((connection) =>
                    {
                        string query = "SELECT fightable FROM cards WHERE owner = @username AND cardid = @id";
                        using NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
                        cmd.Parameters.AddWithValue("username", user.Credentials.Username);
                        cmd.Parameters.AddWithValue("id", id);
                        cmd.Prepare();
                        using NpgsqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            if (reader.GetBoolean("fightable"))
                            {
                                return true;
                            }
                        }
                        //fightable = false => Card nicht bei aktuellem Trade oder Karte existiert nicht oder user ist nicht Owner der Karte

                        return false;
                    });
                    //Wenn owner id nicht hat dann unmöglich zu configuren
                    if (!possibleToAdd)
                    {
                        return false;
                    }
                }

                //remove old cards in deck 
                updatingWorks = ExecuteWithDbConnection((connection) =>
                {
                    string query = "UPDATE cards SET deck = false WHERE owner = @username AND deck = true";
                    using NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("username", user.Credentials.Username);
                    cmd.Prepare();

                    if (cmd.ExecuteNonQuery() != -1)
                    {
                        return true;
                    }
                    return false;

                });

                if (!updatingWorks)
                {
                    throw new DataUpdateException();
                }

                //Wenn Deck configure möglich -> set bool d = true
                foreach (Guid id in guids)
                {
                    updatingWorks = ExecuteWithDbConnection((connection) =>
                    {
                        //Liste zu kaufenden Karte
                        string query = "UPDATE cards SET deck = true WHERE cardid = @id";
                        using NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
                        cmd.Parameters.AddWithValue("id", id);
                        cmd.Prepare();
                        if (cmd.ExecuteNonQuery() != 1)
                        {
                            return false;
                        }
                        return true; ;
                    });
                    if (!updatingWorks)
                    {
                        throw new DataUpdateException();
                    }
                }
                return updatingWorks;
            }
            catch (Exception ex)
            {
                if (ex is DataUpdateException) { throw; }
                else if (ex is DataAccessFailedException) { throw; }
                else if (ex is NpgsqlException) { throw; }
                else { throw; }
            }
        }

        public List<Card> GetDeck(User user)
        {
            string[] spells = { "WaterSpell", "FireSpell", "RegularSpell" };
            List<Card> stack = new List<Card>();
            try
            {
                return ExecuteWithDbConnection((connection) =>
                {
                    string query = "SELECT cardid, name, damage FROM cards WHERE owner = @username AND deck = true";
                    using NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("username", user.Credentials.Username);
                    cmd.Prepare();
                    using NpgsqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (Array.Exists(spells, spell => spell == reader.GetString("name")))
                        {
                            stack.Add(new SpellCard(reader.GetGuid("cardid"), reader.GetString("name"), reader.GetDecimal("damage")));
                        }
                        else
                        {
                            stack.Add(new MonsterCard(reader.GetGuid("cardid"), reader.GetString("name"), reader.GetDecimal("damage")));
                        }
                    }
                    return stack;
                });
            }
            catch (DataAccessFailedException)
            {
                throw;
            }
        }

        public List<Card> GetStack(User user)
        {
            string[] spells = { "WaterSpell", "FireSpell", "RegularSpell" };
            List<Card> stack = new List<Card>();
            try
            {
                return ExecuteWithDbConnection((connection) =>
                {
                    string query = "SELECT cardid, name, damage FROM cards WHERE owner = @username";
                    using NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("username", user.Credentials.Username);
                    cmd.Prepare();
                    using NpgsqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (Array.Exists(spells, spell => spell == reader.GetString("name")))
                        {
                            stack.Add(new SpellCard(reader.GetGuid("cardid"), reader.GetString("name"), reader.GetDecimal("damage")));
                        }
                        else
                        {
                            stack.Add(new MonsterCard(reader.GetGuid("cardid"), reader.GetString("name"), reader.GetDecimal("damage")));
                        }
                    }
                    return stack;
                });
            }
            catch (DataAccessFailedException)
            {
                throw;
            }
        }
    }
}
