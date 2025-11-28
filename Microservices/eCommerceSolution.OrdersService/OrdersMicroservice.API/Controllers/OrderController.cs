using BusinessLogicLayer.DTO;
using BusinessLogicLayer.ServiceContracts;
using DataAccessLayer.Entites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace OrdersMicroservice.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [Route("GetOrders")]

        public async Task<IEnumerable<OrderResponse?>> GetOrders()
        {
            List<OrderResponse?> orders = await _orderService.GetOrders();
            return orders;
        }

        [HttpGet]
        [Route("GetOrderByCondition:{orderID}")]
        public async Task<OrderResponse?> GetOrderById(Guid orderID)
        {
            FilterDefinition<Order> filter = Builders<Order>.Filter.Eq(temp => temp.OrderID, orderID);
            OrderResponse? order = await _orderService.GetOrderByCondition(filter);
            return order;

        }

        [HttpGet]
        [Route("GetOrdersbyProductId:{ProductID}")]
        public async Task<IEnumerable<OrderResponse?>> GetOrdersByProductId(Guid ProductID)
        {
            FilterDefinition<Order> filter = Builders<Order>.Filter.ElemMatch(temp => temp.OrderItems,
                Builders<OrderItem>.Filter.Eq(product => product.ProductID, ProductID));

            List<OrderResponse?> orders = await _orderService.GetOrdersByCondition(filter);
            return orders;

        }


        [HttpGet]
        [Route("GetOrdersByOrderDate:{orderDate}")]
        public async Task<IEnumerable<OrderResponse?>> GetOrdersByDate(DateTime orderDate)
        {
            var startDate = orderDate.Date; // 00:00 of the day
            var endDate = startDate.AddDays(1); // 00:00 of the next day

            var filter = Builders<Order>.Filter.Gte(o => o.OrderDate, startDate) &
                         Builders<Order>.Filter.Lt(o => o.OrderDate, endDate);

            List<OrderResponse?> orders = await _orderService.GetOrdersByCondition(filter);
            return orders;
        }


        [HttpPost]
        [Route("CreateOrders")]
        public async Task<IActionResult> Createorders(OrderAddRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid data");
            }

            OrderResponse order = await _orderService.AddOrder(request);
            return Ok(order);


        }

        [HttpPut]
        [Route("UpdateOrders:{OrderId}")]
        public async Task<IActionResult> Updateorders(OrderUpdateRequest request, Guid OrderId)
        {
            if (request == null)
            {
                return BadRequest("Invalid data");

            }

            if (OrderId != request.OrderID)
            {
                return BadRequest("orderId mismatch");
            }
            OrderResponse? orderResponse = await _orderService.UpdateOrder(request);
            return Ok(orderResponse);
        }

        [HttpDelete]
        [Route("DeleteOrder:{OrderId}")]
        public async Task<IActionResult> DeleteOrder(Guid orderId)
        {
            if (orderId == Guid.Empty)
            {
                return NoContent();
            }
            bool isDeleted = (bool)await _orderService.DeleteOrder(orderId);
            if (!isDeleted)
            {
                return Problem("Error in deletion");
            }
            return Ok(isDeleted);


        }
    }
}