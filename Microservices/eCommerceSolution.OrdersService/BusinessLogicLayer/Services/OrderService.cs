using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLogicLayer.DTO;
using BusinessLogicLayer.HttpClients;
using BusinessLogicLayer.ServiceContracts;
using DataAccessLayer.Entites;
using DataAccessLayer.RepositoryContracts;
using FluentValidation;
using FluentValidation.Results;
using MongoDB.Driver;

namespace BusinessLogicLayer.Services
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;
        private readonly UsersMicroserviceClient _usersMicroserviceClient;
        private readonly IValidator<OrderAddRequest> _orderAddRequestValdator;
        private readonly IValidator<OrderItemAddRequest> _orderItemAddRequestValidator;
        private readonly IValidator<OrderUpdateRequest> _orderUpdateValidator;
        private readonly IValidator<OrderItemUpdateRequest> _orderItemUpdateRequestValdator;

        public OrderService(
            IOrderRepository orderRepository, 
            IMapper mapper,
            IValidator<OrderAddRequest> orderAddRequestValidator,
            IValidator<OrderItemAddRequest> orderItemAddRequestValidator,
            IValidator<OrderUpdateRequest> orderUpdateValidator,
            IValidator<OrderItemUpdateRequest> orderItemUpdateValidator,
            UsersMicroserviceClient usersMicroserviceClient
            )
        {
            _mapper = mapper;
            _orderRepository = orderRepository;
            _orderAddRequestValdator = orderAddRequestValidator;
            _orderItemAddRequestValidator = orderItemAddRequestValidator;
            _orderUpdateValidator = orderUpdateValidator;
            _orderItemUpdateRequestValdator = orderItemUpdateValidator;
            _usersMicroserviceClient = usersMicroserviceClient;

            
        }
        public async Task<OrderResponse?> AddOrder(OrderAddRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            ValidationResult result = await _orderAddRequestValdator.ValidateAsync(request);
            if (!result.IsValid) {
                string errors = string.Join(", ", result.Errors.Select(temp => temp.ErrorMessage));
                throw new ArgumentException(errors);

            }

            foreach (OrderItemAddRequest orderItems in request.OrderItems) {

                ValidationResult orderItemValidation = await _orderItemAddRequestValidator.ValidateAsync(orderItems);
                if (!orderItemValidation.IsValid) {

                    string errors = string.Join(", ", orderItemValidation.Errors.Select(temp => temp.ErrorMessage));
                    throw new ArgumentException(errors);
                }

            }
            // we might need to check the user Id of the users but that
            // is being handled by another Microservice 
            // that code will be written here

           UserDTO? user =  await _usersMicroserviceClient.GetUserByUserID(request.UserID);
            if (user == null) { 
            
            throw new ArgumentNullException(nameof(user));
            }





            //  continuation
            Order neworder = _mapper.Map<Order>(request);
            foreach (OrderItem item in neworder.OrderItems) {

                item.TotalPrice = item.Quantity * item.UnitPrice;


            }
            neworder.TotalBill = neworder.OrderItems.Sum(temp => temp.TotalPrice);

            // adding in the database

            Order addedorder = await _orderRepository.AddOrder(neworder);
            if (addedorder == null)
            {
                return null;
            }
            OrderResponse response = _mapper.Map<OrderResponse>(addedorder);
            return response;
             








        }

        public async Task<bool?> DeleteOrder(Guid OrderID)
        {

            var filter = Builders<Order>.Filter.Eq(o=>o.OrderID , OrderID);            
            Order exorder = await _orderRepository.GetOrderByCondition(filter);
            if(exorder == null)
            {
                return false;
            }
            var isDeleted = await _orderRepository.DeleteOperation(exorder.OrderID);
            return isDeleted;
        }

        public async Task<OrderResponse?> GetOrderByCondition(FilterDefinition<Order> filter)
        {
            Order order = await _orderRepository.GetOrderByCondition(filter);
            if (order == null) { 
            return null;
            }
            OrderResponse response = _mapper.Map<OrderResponse?>(order);
            return response;
        }

        public async Task<List<OrderResponse?>> GetOrders()
        {
            IEnumerable<Order> order = await _orderRepository.GetOrders();
            if (order == null) {
                return null;
            }
            IEnumerable<OrderResponse> response = _mapper.Map<IEnumerable<OrderResponse?>>(order);
            return response.ToList();
        }

        public async Task<List<OrderResponse?>> GetOrdersByCondition(FilterDefinition<Order> filter)
        {
            IEnumerable<Order> order = await _orderRepository.GetOrdersByCondition(filter);
            
            IEnumerable<OrderResponse> response = _mapper.Map<IEnumerable<OrderResponse?>>(order);
            return response.ToList();
        }

        public async Task<OrderResponse?> UpdateOrder(OrderUpdateRequest request)
        {
            if(request == null) {  throw new ArgumentNullException(nameof(request)); }

            ValidationResult validationResult = await _orderUpdateValidator.ValidateAsync(request);
            if (!validationResult.IsValid) {

                string errors = string.Join(", ", validationResult.Errors.Select(temp => temp.ErrorMessage));
                throw new ArgumentException(errors);    
            }
            foreach (OrderItemUpdateRequest orderitems in request.OrderItems) {

                ValidationResult orderupdateItemValidation = await _orderItemUpdateRequestValdator.ValidateAsync(orderitems);
                if (!orderupdateItemValidation.IsValid)
                {

                    string errors = string.Join(", ", orderupdateItemValidation.Errors.Select(temp => temp.ErrorMessage));
                    throw new ArgumentException(errors);
                }

            }
            // different microservice => communicating with the users microservice
            UserDTO? user = await _usersMicroserviceClient.GetUserByUserID(request.UserID);
            if (user == null)
            {

                throw new ArgumentNullException(nameof(user));
            }





            Order orderInput = _mapper.Map<Order>(request);

            foreach (OrderItem orderItem in orderInput.OrderItems) { 
                orderItem.TotalPrice = orderItem.UnitPrice * orderItem.Quantity;
            
            }
            orderInput.TotalBill = orderInput.OrderItems.Sum(x => x.TotalPrice);

            Order updatedOrder = await _orderRepository.UpdateOrder(orderInput);
            if (updatedOrder == null) {

                return null;
            }
            OrderResponse response = _mapper.Map<OrderResponse>(updatedOrder);
            return response;



            
        }
    }
}
 