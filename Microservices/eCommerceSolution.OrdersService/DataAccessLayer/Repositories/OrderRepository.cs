using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Entites;
using DataAccessLayer.RepositoryContracts;
using MongoDB.Driver;

namespace DataAccessLayer.Repositories
{
    public class OrderRepository : IOrderRepository

    {
        private readonly IMongoCollection<Order> _orders;
        private readonly string collectionName = "orders";
        public OrderRepository(IMongoDatabase mongoDatabase)
        {
           _orders =  mongoDatabase.GetCollection<Order>(collectionName);
        }
        public async Task<Order> AddOrder(Order order)
        {
            order.OrderID = Guid.NewGuid();
            order._id = order.OrderID;

            foreach (OrderItem orderItem in order.OrderItems) {
            
                orderItem._id = Guid.NewGuid();
            }

            await _orders.InsertOneAsync(order);
            return order;

        }

        public async Task<bool> DeleteOperation(Guid orderID)
        {
            FilterDefinition<Order> filter =  Builders<Order>.Filter.Eq(temp=>temp.OrderID,orderID);
            Order? exorder = (Order?)await _orders.FindAsync(filter);
            if (exorder == null)
            {
                return false;
            }
            DeleteResult deleteResult =await _orders.DeleteOneAsync(filter);
            return true;


        }

       
        public async Task<Order> GetOrderByCondition(FilterDefinition<Order> filter)
        {
            return (await _orders.FindAsync(filter)).FirstOrDefault();
        }
        public async Task<IEnumerable<Order>> GetOrders()
        {
            return (await _orders.FindAsync(Builders<Order>.Filter.Empty)).ToList();
        }

        public async Task<IEnumerable<Order?>> GetOrdersByCondition(FilterDefinition<Order> filter)
        {
            return (await _orders.FindAsync(filter)).ToList();
        }

        public async Task<Order> UpdateOrder(Order order)
        {
            FilterDefinition<Order> filter = Builders<Order>.Filter.Eq(temp => temp.OrderID, order.OrderID);
            Order? exorder = (await _orders.FindAsync(filter)).FirstOrDefault();
            if (exorder == null) {
                return null;
            }
            order._id = exorder.OrderID; // i don;t need to update the existing orderID
            ReplaceOneResult replaceOneResult = await _orders.ReplaceOneAsync(filter, order);
            return order;

        }
    }
}
