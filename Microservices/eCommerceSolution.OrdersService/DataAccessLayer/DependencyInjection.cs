

using DataAccessLayer.Repositories;
using DataAccessLayer.RepositoryContracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace DataAccessLayer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
        {
            // adding services here

            string connectionstringtemplate = configuration.GetConnectionString("MongoDB")!;
            string connectionstring = connectionstringtemplate.Replace("$MONGO_HOST", Environment.GetEnvironmentVariable("MONGO_HOST"))
                .Replace("MONGO_PORT",Environment.GetEnvironmentVariable("MONGO_PORT"));
            services.AddSingleton<IMongoClient>(new MongoClient(connectionstring));

            services.AddScoped<IMongoDatabase>(
                provider =>{
                IMongoClient client = provider.GetRequiredService<IMongoClient>();
                    //using mongoclient object , we can access the database
                    return client.GetDatabase("OrdersDatabase");
                
            });
            services.AddScoped<IOrderRepository, OrderRepository>();

            return services;

        }
    }
}