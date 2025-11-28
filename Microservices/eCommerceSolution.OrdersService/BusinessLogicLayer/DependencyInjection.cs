using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogicLayer.Mappers;
using BusinessLogicLayer.ServiceContracts;
using BusinessLogicLayer.Services;
using BusinessLogicLayer.Validators;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLogicLayer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            services.AddValidatorsFromAssemblyContaining<OrderAddRequestValidator>();
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<OrderAddRequestToOrderMappingProfile>();
                cfg.AddProfile<OrderItemAddRequestToOrderItemMappingProfile>();
                cfg.AddProfile<OrderItemToOrderItemResponseMapping>();
                cfg.AddProfile<OrderItemUpdateRequestToOrderItemMappingProfile>();
                cfg.AddProfile<OrderToOrderResponseMappingProfile>();
                cfg.AddProfile<OrderUpdateRequestToOrderMappingProfile>();
            });
            services.AddScoped<IOrderService, OrderService>();
            return services;
        }
    }
}
