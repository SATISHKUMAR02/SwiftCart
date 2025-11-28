using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using BusinessLogicLayer.DTO;
using Microsoft.AspNetCore.Http;

namespace BusinessLogicLayer.HttpClients
{
    public class UsersMicroserviceClient
    {
        private readonly HttpClient _httpClient;

        public UsersMicroserviceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<UserDTO?> GetUserByUserID(Guid userID)
        {


            HttpResponseMessage response = await _httpClient.GetAsync($"/api/Users/GetUserByID/{userID}");

            if (!response.IsSuccessStatusCode)
            {
                if(response.StatusCode == System.Net.HttpStatusCode.NotFound)
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
                
         
            }

            
            UserDTO? user = await response.Content.ReadFromJsonAsync<UserDTO>();
            if(user == null)
            {
                throw new ArgumentException("Invalid User");
            }   
            return user;
        }
    }
}
