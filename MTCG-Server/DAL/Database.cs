using MTCGServer.DAL.Cards;
using MTCGServer.DAL.Exceptions;
using MTCGServer.DAL.Game;
using MTCGServer.DAL.Packages;
using MTCGServer.DAL.Trading;
using MTCGServer.DAL.Users;
using Npgsql;

namespace MTCGServer.DAL
{
    public class Database
    {
        // see also https://www.postgresql.org/docs/current/ddl-constraints.html
        private const string CreateTablesCommand = @"
        CREATE TABLE IF NOT EXISTS public.cards
        (
            cardid uuid NOT NULL,
            fightable boolean NOT NULL,
            element text COLLATE pg_catalog.""default"" NOT NULL,
            name text COLLATE pg_catalog.""default"" NOT NULL,
            damage numeric NOT NULL,
            owner text COLLATE pg_catalog.""default"" NOT NULL,
            deck boolean NOT NULL DEFAULT false,
            CONSTRAINT cards_pkey PRIMARY KEY (cardid)
        )

        TABLESPACE pg_default;

        ALTER TABLE IF EXISTS public.cards
            OWNER to postgres;

        CREATE TABLE IF NOT EXISTS public.match_cards_to_package
        (
            packageid bigint NOT NULL,
            cardid uuid NOT NULL,
            CONSTRAINT ""matchCardsToPackage_pkey"" PRIMARY KEY (cardid)
        )

        TABLESPACE pg_default;

        ALTER TABLE IF EXISTS public.match_cards_to_package
            OWNER to postgres;

        CREATE TABLE IF NOT EXISTS public.packages
        (
            packageid bigint NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 9223372036854775807 CACHE 1 ),
            bought boolean NOT NULL,
            CONSTRAINT packages_pkey PRIMARY KEY (packageid)
        )

        TABLESPACE pg_default;

        ALTER TABLE IF EXISTS public.packages
            OWNER to postgres;

        CREATE TABLE IF NOT EXISTS public.trades
        (
            tradeid uuid NOT NULL,
            username text COLLATE pg_catalog.""default"" NOT NULL,
            cardid uuid NOT NULL,
            mindamage numeric NOT NULL,
            type text COLLATE pg_catalog.""default"" NOT NULL,
            CONSTRAINT trades_pkey PRIMARY KEY (tradeid)
        )

        TABLESPACE pg_default;

        ALTER TABLE IF EXISTS public.trades
            OWNER to postgres;

        CREATE TABLE IF NOT EXISTS public.users
        (
            username text COLLATE pg_catalog.""default"" NOT NULL,
            password text COLLATE pg_catalog.""default"" NOT NULL,
            money integer NOT NULL,
            name text COLLATE pg_catalog.""default"",
            bio text COLLATE pg_catalog.""default"",
            image text COLLATE pg_catalog.""default"",
            wins smallint NOT NULL,
            losses smallint NOT NULL,
            elo smallint NOT NULL,
            token text COLLATE pg_catalog.""default"",
            CONSTRAINT ""Users_pkey"" PRIMARY KEY (username)
        )

        TABLESPACE pg_default;

        ALTER TABLE IF EXISTS public.users
            OWNER to postgres;
";

        public IUserDao UserDao { get; private set; }
        public IPackageDao PackageDao { get; private set; }
        public ICardDao CardDao { get; private set; }
        public IGameDao GameDao { get; private set; }

        public ITradingDao TradeDao { get; private set; }

        public Database(string connectionString)
        {
            try
            {
                try
                {
                    // https://github.com/npgsql/npgsql/issues/1837https://github.com/npgsql/npgsql/issues/1837
                    // https://www.npgsql.org/doc/basic-usage.html
                    // https://www.npgsql.org/doc/connection-string-parameters.html#pooling
                    EnsureTables(connectionString);
                }
                catch (NpgsqlException e)
                {
                    // provide our own custom exception
                    throw new DataAccessFailedException("Could not connect to or initialize database", e);
                }
                UserDao = new DatabaseUserDao(connectionString);
                PackageDao = new DatabasePackageDao(connectionString);
                CardDao = new DatabaseCardDao(connectionString);
                GameDao = new DatabaseGameDao(connectionString);
                TradeDao = new DatabaseTradingDao(connectionString);
            }
            catch (NpgsqlException e)
            {
                // provide our own custom exception
                throw new DataAccessFailedException("Could not connect to or initialize database", e);
            }
        }

        private void EnsureTables(string connectionString)
        {
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand(CreateTablesCommand, connection);
            cmd.ExecuteNonQuery();
        }
    }
}
