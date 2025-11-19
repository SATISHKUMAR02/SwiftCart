using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogicLayer.DTO;
using DataAccessLayer.Entites;
using MongoDB.Driver;

namespace BusinessLogicLayer.ServiceContracts
{
    public interface IOrderService
    {
        Task<List<OrderResponse?>> GetOrders();

        Task<List<OrderResponse?>> GetOrdersByCondition(FilterDefinition<Order> filter);

        Task<OrderResponse?> GetOrderByCondition(FilterDefinition<Order> filter);

        Task<OrderResponse?> AddOrder(OrderAddRequest request);

        Task<OrderResponse?> UpdateOrder(OrderUpdateRequest request);

        Task<bool?> DeleteOrder(Guid OrderID);

     }
}
