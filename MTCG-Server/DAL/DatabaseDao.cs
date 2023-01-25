using MTCGServer.DAL.Exceptions;
using Npgsql;

namespace SWE1.MessageServer.DAL
{
    public abstract class DatabaseDao
    {
        private readonly string _connectionString;

        public DatabaseDao(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected T ExecuteWithDbConnection<T>(Func<NpgsqlConnection, T> command)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                connection.Open();

                return command(connection);
            }
            catch (NpgsqlException e)
            {
                if (e is PostgresException)
                {
                    throw;
                }
                else
                {
                    throw new DataAccessFailedException("Could not connect to database", e);
                }
            }
        }
    }
}
