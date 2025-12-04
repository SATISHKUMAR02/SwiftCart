using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using BusinessLogicLayer.DTO;

namespace BusinessLogicLayer.HttpClients
{
    public class ProductMicroserviceClient
    {
        private readonly HttpClient _httpClient;
        public ProductMicroserviceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ProductDTO?> GetProductById(Guid productId)
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
            if (product == null) {
                throw new ArgumentException("invalid cred");
            
            }
            return product;


            

        }
    }
}
