using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace eCommerce.Infrastructure.DBContext
{
    public class DapperDbContext
    {
        private readonly IConfiguration _configuration;
        private readonly IDbConnection dbConnection;
        public DapperDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
            string connectionString = _configuration.GetConnectionString("PostgresConnection");

            dbConnection = new NpgsqlConnection(connectionString);

        }
        public IDbConnection DbConnection => dbConnection;
        
    }
}
