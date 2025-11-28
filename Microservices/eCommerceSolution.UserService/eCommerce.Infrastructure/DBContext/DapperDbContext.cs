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
            string connectionStringtemplate = _configuration.GetConnectionString("PostgresConnection")!;
            string connectionString = connectionStringtemplate
                .Replace("$POSTGRES_HOST", Environment.GetEnvironmentVariable("POSTGRES_HOST"))
                .Replace("$POSTGRES_PORT", Environment.GetEnvironmentVariable("POSTGRES_PORT"))
                .Replace("$POSTGRES_USER", Environment.GetEnvironmentVariable("POSTGRES_USER"))
                .Replace("$POSTGRES_PASSWORD", Environment.GetEnvironmentVariable("POSTGRES_PASSWORD"))
                .Replace("$POSTGRES_DATABASE", Environment.GetEnvironmentVariable("POSTGRES_DATABASE"));

            dbConnection = new NpgsqlConnection(connectionString);
        }

        public IDbConnection DbConnection => dbConnection;
        
    }
}
