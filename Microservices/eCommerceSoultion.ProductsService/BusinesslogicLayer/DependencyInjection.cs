using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinesslogicLayer.Mapping;
using BusinesslogicLayer.ServiceContracts;
using BusinesslogicLayer.Services;
using BusinesslogicLayer.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
namespace BusinesslogicLayer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinesslogicLayer(this IServiceCollection services)
        {
            // Register stuff
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<ProductUpdateToProduct>();
                cfg.AddProfile<ProductToProductResponseMapping>();
                cfg.AddProfile<ProductAddToProductMappingProfile>();
            });


            services.AddValidatorsFromAssemblyContaining<ProductAddRequestValidator>();
            services.AddScoped<IProductService, ProductService>();

            return services;
        }
    }
}
