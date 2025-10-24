using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinesslogicLayer.Mapping;
using Microsoft.Extensions.DependencyInjection;
namespace BusinesslogicLayer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinesslogicLayer(this IServiceCollection services)
        {
            // Register stuff
            services.AddAutoMapper(typeof(ProductAddToProductMappingProfile).Assembly);
            return services;
        }
    }
}
