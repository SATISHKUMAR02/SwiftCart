using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using BusinessLogicLayer.DTO;
using DnsClient.Internal;
using Microsoft.Extensions.Logging;
using Polly.Bulkhead;

namespace BusinessLogicLayer.HttpClients
{
    public class ProductMicroserviceClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ProductMicroserviceClient> _logger;
        public ProductMicroserviceClient(HttpClient httpClient,ILogger<ProductMicroserviceClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<ProductDTO?> GetProductById(Guid productId)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"/api/Product/GetSinlgeProducts/{productId}");
                if (!response.IsSuccessStatusCode)

                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
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
                ProductDTO? product = await response.Content.ReadFromJsonAsync<ProductDTO>();
                if (product == null)
                {
                    throw new ArgumentException("invalid cred");

                }
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
