using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Entites;
using MongoDB.Driver;

namespace DataAccessLayer.RepositoryContracts
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetOrders();
        Task<IEnumerable<Order?>> GetOrdersByCondition(FilterDefinition<Order> filter);
        Task <Order> GetOrderByCondition(FilterDefinition<Order> filter);
        Task<Order> AddOrder(Order order);
        Task<bool>DeleteOperation(Guid orderID);
        Task<Order> UpdateOrder(Order order);
    }
}
