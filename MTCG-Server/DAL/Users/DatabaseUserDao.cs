using MTCGServer.DAL.Exceptions;
using MTCGServer.Models;
using Npgsql;
using SWE1.MessageServer.DAL;
using System.Data;
using System.Text;

namespace MTCGServer.DAL.Users
{
    public class DatabaseUserDao : DatabaseDao, IUserDao
    {
        public DatabaseUserDao(string connectionString) : base(connectionString) { }

        public User? GetUserByAuthToken(string authToken)
        {
            User? user = null;
            try
            {
                return ExecuteWithDbConnection((connection) =>
                {
                    string query = "SELECT * FROM users WHERE token = @Token";
                    using NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("token", authToken);
                    cmd.Prepare();

                    using NpgsqlDataReader reader = cmd.ExecuteReader();
                    user = createUser(reader);

                    return user;
                });
            }
            catch (DataAccessFailedException)
            {
                throw;
            }
        }

        public User? GetUserByCredentials(string username, string password)
        {
            User? user = null;
            try
            {
                user = ExecuteWithDbConnection((connection) =>
                {
                    //get User by Credetials out of database
                    //User? user = null;
                    string query1 = "SELECT * FROM users WHERE username=@username AND password=@password";
                    using var cmd1 = new NpgsqlCommand(query1, connection);
                    cmd1.Parameters.AddWithValue("username", username);
                    cmd1.Parameters.AddWithValue("password", password);
                    cmd1.Prepare();

                    using NpgsqlDataReader reader = cmd1.ExecuteReader();
                    user = createUser(reader);

                    return user;
                });



                if (user == null)
                {
                    return user;
                }

                return ExecuteWithDbConnection((connection) =>
                {
                    user.Token = $"{username}-mtcgToken";

                    //insert Token into database
                    string query2 = "UPDATE users SET token = @token WHERE username = @username";
                    using NpgsqlCommand cmd2 = new NpgsqlCommand(query2, connection);
                    cmd2.Parameters.AddWithValue("token", user.Token);
                    cmd2.Parameters.AddWithValue("username", username);
                    cmd2.Prepare();

                    cmd2.ExecuteNonQuery();

                    return user;
                });
            }
            catch (DataAccessFailedException)
            {
                throw;
            }
        }

        public bool InsertUser(User user)
        {
            bool inserted = false;
            try
            {
                inserted = ExecuteWithDbConnection((connection) =>
                {
                    //these colums are not allowed to be null
                    string query = "INSERT INTO users (username, password, money, name, bio, image, wins, losses, elo, token) VALUES (@Username, @Password, @Money, @Name, @Bio, @Image, @Wins, @Losses, @Elo, @token)";
                    using NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("username", user.Credentials.Username);
                    cmd.Parameters.AddWithValue("password", user.Credentials.Password);
                    cmd.Parameters.AddWithValue("money", user.Money);
                    cmd.Parameters.AddWithValue("name", user.UserData.Name);
                    cmd.Parameters.AddWithValue("bio", user.UserData.Bio);
                    cmd.Parameters.AddWithValue("image", user.UserData.Image);
                    cmd.Parameters.AddWithValue("wins", user.ScoreboardData.Wins);
                    cmd.Parameters.AddWithValue("losses", user.ScoreboardData.Losses);
                    cmd.Parameters.AddWithValue("elo", user.ScoreboardData.Elo);
                    cmd.Parameters.AddWithValue("token", user.Token);
                    cmd.Prepare();

                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        inserted = true;
                    }
                    return inserted;
                });
            }
            catch (Exception ex)
            {
                if (ex is DataAccessFailedException)
                {
                    throw;
                }

                else if (ex is PostgresException)
                {
                    throw;
                }

            }

            return inserted;

        }

        private User? GetUserByUsername(string username)
        {
            try
            {
                return ExecuteWithDbConnection((connection) =>
                {
                    User? user = null;
                    string query = "SELECT * FROM users WHERE username = @username";
                    using NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("username", username);
                    cmd.Prepare();

                    using NpgsqlDataReader reader = cmd.ExecuteReader();
                    user = createUser(reader);

                    return user;
                });
            }
            catch (DataAccessFailedException)
            {
                throw;
            }
        }

        private User? createUser(NpgsqlDataReader reader)
        {
            User? user = null;
            if (reader.Read())
            {
                Credentials credentials = new Credentials(reader.GetString("username"), reader.GetString("password"));
                UserData userdata = new UserData(reader.GetString("name"), reader.GetString("bio"), reader.GetString("image"));
                ScoreboardData scoreboardData = new ScoreboardData(reader.GetString("name"), reader.GetInt16("wins"), reader.GetInt16("losses"), reader.GetInt16("elo"));
                user = new User(credentials, reader.GetInt16("money"), userdata, reader.GetString("token"), scoreboardData);
            }
            return user;
        }

        public string? GetToken(string username)
        {
            try
            {
                return ExecuteWithDbConnection((connection) =>
                {
                    string? token = null;
                    string query = "SELECT token FROM users WHERE username = @Username";
                    using NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("username", username);
                    cmd.Prepare();

                    using NpgsqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        token = reader.GetString(0);
                    }

                    return token;
                });
            }
            catch (DataAccessFailedException)
            {
                throw;
            }
        }

        public UserData? GetUserData(string username)
        {
            try
            {
                return ExecuteWithDbConnection((connection) =>
                {
                    UserData? userdata = null;

                    string query = "SELECT name, bio, image FROM users WHERE username = @Username";
                    using NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("username", username);
                    cmd.Prepare();

                    using NpgsqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        userdata = new UserData(reader.GetString("name"), reader.GetString("bio"), reader.GetString("image"));
                    }

                    return userdata;
                });
            }
            catch (DataAccessFailedException)
            {
                throw;
            }
        }

        public bool ExistUser(string username)
        {
            try
            {
                return ExecuteWithDbConnection((connection) =>
                {
                    bool userExist = false;

                    string query = "SELECT username FROM users WHERE username = @username";
                    using NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("username", username);
                    cmd.Prepare();

                    using NpgsqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        userExist = true;
                    }

                    return userExist;
                });
            }
            catch (DataAccessFailedException)
            {
                throw;
            }
        }

        public bool UpdateUserData(UserData userData, string username)
        {
            try
            {
                return ExecuteWithDbConnection((connection) =>
                {
                    bool dataUpdated = false;
                    string query = "UPDATE users SET name = @name, bio = @bio, image = @image WHERE username = @username";
                    using NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("name", userData.Name);
                    cmd.Parameters.AddWithValue("bio", userData.Bio);
                    cmd.Parameters.AddWithValue("image", userData.Image);
                    cmd.Parameters.AddWithValue("username", username);
                    cmd.Prepare();

                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        dataUpdated = true;
                    }
                    return dataUpdated;
                });
            }
            catch (DataAccessFailedException)
            {
                throw;
            }
        }
    }
}
