using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCGServer.Models;
using Npgsql;

namespace MTCGServer.DAL
{
    public class DB : IDB
    {
        NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(@"Server=localhost;Port=10001;User Id=postgres;Password=123;Database=MTCGDB;");
        }
    }
}
