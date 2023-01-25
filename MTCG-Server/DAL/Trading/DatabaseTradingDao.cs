using MTCGServer.DAL.Exceptions;
using MTCGServer.Models;
using Npgsql;
using SWE1.MessageServer.DAL;
using System.Data;
using System.Text;

namespace MTCGServer.DAL.Trading
{
    public class DatabaseTradingDao : DatabaseDao, ITradingDao
    {
        public DatabaseTradingDao(string connectionString) : base(connectionString) { }

        public bool CreateTrade(Trade trade, User user)
        {
            bool createTrade = false;
            try
            {
                //check if the cards owner is the user and if the card is not locked in the deck
                createTrade = ExecuteWithDbConnection((connection) =>
                {
                    string query = "SELECT owner, deck FROM cards WHERE cardid = @cardid";
                    using NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("cardid", trade.CardToTrade);
                    cmd.Prepare();
                    using NpgsqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        if (user.Credentials.Username != reader.GetString("owner") || reader.GetBoolean("deck"))
                        {
                            return false;
                        }
                        return true;
                    }
                    return false;
                });

                if (!createTrade)
                {
                    return false;
                }

                return ExecuteWithDbConnection((connection) =>
                {
                    //Create the list of Cards the user buy 
                    string query = "INSERT INTO trades  (tradeid, username, cardid, mindamage, type) VALUES (@tradeid, @username, @cardid, @mindamage, @type)";
                    using NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("tradeid", trade.Id);
                    cmd.Parameters.AddWithValue("username", user.Credentials.Username);
                    cmd.Parameters.AddWithValue("cardid", trade.CardToTrade);
                    cmd.Parameters.AddWithValue("mindamage", trade.MinimumDamage);
                    cmd.Parameters.AddWithValue("type", trade.Type);
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();

                    return true; //Create new trade works correctly 
                });
            }
            catch (Exception ex)
            {
                if (ex is DataAccessFailedException)
                {
                    throw;
                }
                else if (ex is PostgresException) //if tradeid already exist
                {
                    throw;
                }
                else
                {
                    Console.WriteLine(ex.Message + "unerwartete Exception beim Create Trade");
                    throw;
                }
            }
        }

        public List<Trade> GetTrades()
        {
            List<Trade> trades = new List<Trade>();
            try
            {
                return ExecuteWithDbConnection((connection) =>
                {
                    //Create the list of Cards the user buy 
                    string query = "SELECT tradeid, cardid, type, mindamage FROM trades";
                    using NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
                    cmd.Prepare();
                    using NpgsqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        trades.Add(new Trade(reader.GetGuid("tradeid"), reader.GetGuid("cardid"), reader.GetString("type"), reader.GetDecimal("mindamage")));
                    }
                    return trades;
                });
            }
            catch (DataAccessFailedException)
            {
                throw;
            }
        }

        public bool MakeTrade(Guid tradeId, Guid cardToMakeTrade, User tradeMaker)
        {
            bool tradingPossible = true;
            decimal minDamage = 0;
            string requiredType = "";
            Guid cardForTradeMaker = new Guid();
            Guid cardForTradeCreater = cardToMakeTrade;
            string tradeCreater = ""; //username of the user who created the trade

            try
            {
                bool existTrade = ExecuteWithDbConnection((connection) =>
                {
                    string query = "SELECT username, mindamage, type, cardid FROM trades WHERE tradeid = @tradeid";
                    using NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("tradeid", tradeId);
                    cmd.Prepare();
                    using NpgsqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        minDamage = reader.GetDecimal("mindamage");
                        requiredType = reader.GetString("type");
                        cardForTradeMaker = reader.GetGuid("cardid");
                        tradeCreater = reader.GetString("username");
                        return true;
                    }
                    else
                    {
                        throw new TradeIdNotFoundException();
                    }
                });

                tradingPossible = ExecuteWithDbConnection((connection) =>
                {
                    string query = "SELECT owner, name, damage, deck FROM cards WHERE cardid = @cardid";
                    using NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("cardid", cardToMakeTrade);
                    cmd.Prepare();
                    using NpgsqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        string[] spells = { "WaterSpell", "FireSpell", "RegularSpell" }; //array which card names are from type spell
                        string type; //to safe the type of the card 
                        if (Array.Exists(spells, spell => spell == reader.GetString("name")))
                        {
                            type = "spell";
                        }
                        else
                        {
                            type = "monster";
                        }
                        //            offered card is not owned by the user               |   card not met requirement Damage       |card typ != wanted type | card is locked in the deck | trading with yourself
                        if (reader.GetString("owner") != tradeMaker.Credentials.Username || minDamage > reader.GetDecimal("damage") || requiredType != type || reader.GetBoolean("deck") || tradeCreater == tradeMaker.Credentials.Username)
                        {
                            return false; // => StatusCode 403  offered card is not owned by the user, or the requirements are not met (Type, MinimumDamage)
                                          // or the offered card is locked in the deck, or the user tries to trade with self
                        }
                        return true;
                    }
                    return false; // => cardId don't exist => card is not owned by the user
                });

                if (tradingPossible)
                {
                    if (UpdateOwners(tradeMaker.Credentials.Username, cardForTradeMaker) && UpdateOwners(tradeCreater, cardForTradeCreater))
                    {
                        if (!DeleteTradeAfterTrading(tradeId))
                        {
                            throw new DataDeletingFailsException();
                        }
                    }
                    else
                    {
                        throw new DataUpdateException();
                    }
                }
                //if everything worked correct
                return tradingPossible;
            }
            catch (Exception ex)
            {
                if (ex is TradeIdNotFoundException) { throw; }
                if (ex is DataAccessFailedException) { throw; }
                if (ex is DataDeletingFailsException) { throw; }
                if (ex is DataUpdateException) { throw; }
                else { throw; }
            }
        }

        public bool DeleteTradeAfterTrading(Guid tradeId)
        {
            try
            {
                Guid cardIdFromTrade = GetCardId(tradeId);
                if (cardIdFromTrade != Guid.Empty)
                {
                    if (UpdateFightable(cardIdFromTrade))
                    {
                        return ExecuteWithDbConnection((connection) =>
                        {
                            //Create the list of Cards the user buy 
                            string query = "DELETE FROM trades WHERE tradeid = @tradeid";
                            using NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
                            cmd.Parameters.AddWithValue("tradeid", tradeId);
                            cmd.Prepare();

                            if (cmd.ExecuteNonQuery() == 1)
                            {
                                return true;
                            }
                            else
                            {
                                throw new DataDeletingFailsException(); //Execute non query liefer != 1 zurück
                            }
                        });
                    }
                    else
                    {
                        throw new DataUpdateException();
                    }
                }
                //tradeid does not exist
                throw new TradeIdNotFoundException();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (ex is DataAccessFailedException) { throw; }
                if (ex is TradeIdNotFoundException) { throw; }
                if (ex is DataDeletingFailsException) { throw; }
                if (ex is DataUpdateException) { throw; }
                else { throw; }
            }
        }

        public bool DeleteTrade(Guid tradeId, string tradeCreater)
        {
            try
            {
                Guid cardIdFromTrade = GetCardId(tradeId);
                if (cardIdFromTrade != Guid.Empty)
                {
                    bool worksFine = checkOwnerOfCard(cardIdFromTrade, tradeCreater); //returns true if the owner of the card == the person who created the trade
                    if (worksFine)
                    {
                        if (UpdateFightable(cardIdFromTrade))
                        {
                            return ExecuteWithDbConnection((connection) =>
                            {
                                //Create the list of Cards the user buy 
                                string query = "DELETE FROM trades WHERE tradeid = @tradeid";
                                using NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
                                cmd.Parameters.AddWithValue("tradeid", tradeId);
                                cmd.Prepare();

                                if (cmd.ExecuteNonQuery() == 1)
                                {
                                    return true;
                                }
                                else
                                {
                                    throw new DataDeletingFailsException(); //Execute non query liefer != 1 zurück
                                }
                            });
                        }
                    }
                    else
                    {
                        //The deal contains a card that is not owned by the user. => 403
                        return false;
                    }

                }
                //tradeid does not exist
                throw new TradeIdNotFoundException();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (ex is DataAccessFailedException) { throw; }
                else { throw; }
            }
        }
        private bool checkOwnerOfCard(Guid cardIdFromTrade, string tradeCreater)
        {
            try
            {
                return ExecuteWithDbConnection((connection) =>
                {
                    //Create the list of Cards the user buy 
                    string query = "SELECT owner FROM cards WHERE cardid = @cardid";
                    using NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("cardid", cardIdFromTrade);
                    cmd.Prepare();
                    using NpgsqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        if (tradeCreater == reader.GetString("owner"))
                        {
                            return true;
                        }
                    }
                    return false;

                });
            }
            catch (Exception ex)
            {
                if (ex is DataAccessFailedException) { throw; }
                else { throw; }
            }
        }

        private bool UpdateOwners(string username, Guid newCardForUser)
        {
            try
            {
                return ExecuteWithDbConnection((connection) =>
                {
                    //Create the list of Cards the user buy 
                    string query = "UPDATE cards SET owner = @user1 WHERE cardid = @cardid";
                    using NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("user1", username);
                    cmd.Parameters.AddWithValue("cardid", newCardForUser);
                    cmd.Prepare();

                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                });
            }
            catch (Exception ex)
            {
                if (ex is DataAccessFailedException) { throw; }
                else { throw; }
            }
        }

        private bool UpdateFightable(Guid cardId)
        {
            try
            {
                return ExecuteWithDbConnection((connection) =>
                {
                    //Create the list of Cards the user buy 
                    string query = "UPDATE cards SET fightable = true WHERE cardid = @cardid";
                    using NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("cardid", cardId);
                    cmd.Prepare();
                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        return true;
                    }
                    return false;
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (ex is DataAccessFailedException) { throw; }
                else { throw; }
            }
        }

        private Guid GetCardId(Guid tradeId)
        {
            Guid cardId = Guid.Empty;
            try
            {
                return ExecuteWithDbConnection((connection) =>
                {
                    string query = "SELECT cardid FROM trades WHERE tradeid = @tradeid";
                    using NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("tradeid", tradeId);
                    cmd.Prepare();
                    using NpgsqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        cardId = reader.GetGuid("cardid");
                    }
                    return cardId;
                });
            }
            catch (Exception ex)
            {
                if (ex is DataAccessFailedException) { throw; }
                else
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }

        }
    }
}
