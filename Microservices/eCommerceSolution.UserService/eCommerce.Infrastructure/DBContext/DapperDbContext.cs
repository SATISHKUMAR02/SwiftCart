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
            string connectionStringtemplate = _configuration.GetConnectionString("PostgresConnection")!; // cannot be null
            string connectionString = connectionStringtemplate.Replace("$POSTGRES_HOST", Environment.GetEnvironmentVariable("POSTGRES_HOST"))
                .Replace("$POSTGRES_PASSWORD", Environment.GetEnvironmentVariable("POSTGRES_PASSWORD"));
            dbConnection = new NpgsqlConnection(connectionString);

        }
        public IDbConnection DbConnection => dbConnection;
        
    }
}
