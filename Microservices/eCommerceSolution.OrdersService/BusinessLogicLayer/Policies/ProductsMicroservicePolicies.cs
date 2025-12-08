using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using BusinessLogicLayer.DTO;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Bulkhead;
using Polly.CircuitBreaker;
using Polly.Fallback;
using Polly.Retry;
using Polly.Timeout;
using Polly.Wrap;

namespace BusinessLogicLayer.Policies
{
    public class ProductsMicroservicePolicies : IProductMicroservicePolicies
    {
        private readonly ILogger<ProductsMicroservicePolicies> _logger;

        public ProductsMicroservicePolicies(ILogger<ProductsMicroservicePolicies> logger)
        {
            _logger = logger;
        }

        public IAsyncPolicy<HttpResponseMessage> GetBulkHeadPolicy()
        {
            AsyncBulkheadPolicy<HttpResponseMessage> policy = Policy.BulkheadAsync<HttpResponseMessage>(
                maxParallelization: 2, // this will allow upto two concurrent requests
                maxQueuingActions: 40,
                // if the max no of requests exceeds more than two , those requests will be kept in the queue which 
                // has max capacity of 40 in FIFO order
                onBulkheadRejectedAsync: (context)=>
                {
                    _logger.LogWarning("BulkheadIsolation Triggered , can send any more requests");
                    throw new BulkheadRejectedException("Bulkhead queue is full");
                }

          
                );
            return policy;
        }

       public  IAsyncPolicy<HttpResponseMessage> GetFallbackPolicy()
        {
            AsyncFallbackPolicy<HttpResponseMessage> policy = Policy
                .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .FallbackAsync(async (context) =>
                {
                    _logger.LogWarning("Fallback triggered. Returning dummy product data.");

                    ProductDTO product = new ProductDTO(
                        ProductID: Guid.Empty,
                        ProductName: "Temporarily unavailable fallback",
                        Quantity: 0,
                        Category: "Temporarily unavailable fallback",
                        UnitPrice: 0
                    );

                    var response = new HttpResponseMessage(System.Net.HttpStatusCode.ServiceUnavailable)
                    {
                        Content = new StringContent(JsonSerializer.Serialize(product),
                        Encoding.UTF8,
                        "application/json")
                    };

                    return response;
                });

            return policy;
        }

        public IAsyncPolicy<HttpResponseMessage> GetCombinedPolicy()
        {
            var getFallbackPolicy = GetFallbackPolicy();
            var getBulkHeadPolicy = GetBulkHeadPolicy();
            AsyncPolicyWrap<HttpResponseMessage> policies = Policy.WrapAsync(getFallbackPolicy,getBulkHeadPolicy);
            return policies;


        }
    }
}
