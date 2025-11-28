using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Context;
using DataAccessLayer.Repositories;
using DataAccessLayer.RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace DataAccessLayer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataAccessLayer(this IServiceCollection services,IConfiguration configuration)
        {
            string connectinoStringTemplate = configuration.GetConnectionString("DefaultConnection")!;
            string connectionstring =  connectinoStringTemplate.Replace("$MSSQL_HOST", Environment.GetEnvironmentVariable("MSSQL_HOST"))
                .Replace("$MSSQL_PASSWORD", Environment.GetEnvironmentVariable("MSSQL_PASSWORD"))
                .Replace("$MSSQL_PORT", Environment.GetEnvironmentVariable("MSSQL_PORT"))
                .Replace("$MSSQL_DATABASE", Environment.GetEnvironmentVariable("MSSQL_DATABASE"));
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionstring,sqlServerOptions => sqlServerOptions.EnableRetryOnFailure(
                    
                    maxRetryCount:5,
                    maxRetryDelay:TimeSpan.FromSeconds(10),
                    errorNumbersToAdd:null
                    
                    ));
            });
            services.AddScoped<IProductsRepository, ProductsRepository>();
            return services;
        }
    }
}
