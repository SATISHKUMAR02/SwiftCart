using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BusinessLogicLayer.DTO;
using DnsClient.Internal;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Polly.Bulkhead;

namespace BusinessLogicLayer.HttpClients
{
    public class ProductMicroserviceClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ProductMicroserviceClient> _logger;
        private readonly IDistributedCache _distributedCache;
        public ProductMicroserviceClient(HttpClient httpClient,ILogger<ProductMicroserviceClient> logger,IDistributedCache distributedCache)
        {
            _httpClient = httpClient;
            _logger = logger;
            _distributedCache = distributedCache;
        }

        public async Task<ProductDTO?> GetProductById(Guid productId)
        {
            try
            {
                // we are checking if that data is in cache
                // we first see if it is in cache , if present m send from cache to reduce time , 
                // if not send a request and fetch and store it

                string cacheKey = $"product:{productId}";
                string cachcedProduct =  await  _distributedCache.GetStringAsync(cacheKey);
                if(cachcedProduct != null)
                {
                    ProductDTO productFromCache  = JsonSerializer.Deserialize<ProductDTO>(cachcedProduct);
                    return productFromCache;
                }

                HttpResponseMessage response = await _httpClient.GetAsync($"/gateway/Products/GetSingleProduct/{productId}");
                // sending a request through API Gateway than sending it to product microservice directly
                if (!response.IsSuccessStatusCode)

                    if(response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
                    {// not to add fallback exception in cache - VVVVV important
                        ProductDTO? productFromFallback = await response.Content.ReadFromJsonAsync<ProductDTO>();
                        if (productFromFallback == null) {
                            throw new NotImplementedException("Fallback Policy was not implemented");
                        }
                        return productFromFallback;
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        return null;
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {

                        throw new HttpRequestException("Bad Request", null, System.Net.HttpStatusCode.BadRequest);
                    }
                    else
                    {
                        throw new HttpRequestException($"failed {response.StatusCode}");
                    }
                ProductDTO? product = await response.Content.ReadFromJsonAsync<ProductDTO>(); // converting to json and this can be cached
                if (product == null)
                {
                    throw new ArgumentException("invalid cred");

                }

                // adding the product to cache so that next 
                // Key:product:{productID}
                //value:{"ProductName":"","Category":""} this is how the data is stored in cache

                string productJson =  JsonSerializer.Serialize(product);

                //create Cache Options to specify the life to cache data in the application
                DistributedCacheEntryOptions options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(2))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(1));
                string cacheKeyToWrite = $"product:{productId}";
                await _distributedCache.SetStringAsync(cacheKeyToWrite, productJson,options);

                return product;
            }catch (BulkheadRejectedException ex)
            {
                _logger.LogError(ex, "Bulkhead Isolation blocks request since queue is full");
                return new ProductDTO(
                    ProductID: Guid.Empty,
                    ProductName: "Temperarily Unavailable (Bulkhead Exception)",
                    Quantity: 0,
                    UnitPrice:0.0,
                    Category:"Temporarily Unavailable (Bulkhead Exception)"

                    );
                
            }


            

        }
    }
}
