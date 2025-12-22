using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BusinessLogicLayer.DTO;
using DnsClient.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Polly.CircuitBreaker;

namespace BusinessLogicLayer.HttpClients
{
    public class UsersMicroserviceClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<UsersMicroserviceClient> _logger;
        private readonly IDistributedCache _distributedCache;

        

        public UsersMicroserviceClient(HttpClient httpClient, ILogger<UsersMicroserviceClient> logger,IDistributedCache distributedCache)
        {
            _httpClient = httpClient;
            _logger = logger;
            _distributedCache = distributedCache;
        }
        public async Task<UserDTO?> GetUserByUserID(Guid userID)
        {

            try
            {

                // cache 
                string cacheKey = $"user:{userID}";
                string cachcedUser = await _distributedCache.GetStringAsync(cacheKey);
                if (cachcedUser != null)
                {
                    UserDTO userFromCache = JsonSerializer.Deserialize<UserDTO>(cachcedUser);
                    return userFromCache;
                }




                // the use of Polly will be reflected here as before the httpclient completes the request and 
                // before passing it response , it will execute the code in Program.CS file where the polly 
                // method will execute to verify if the other microservice might need to send another retry 
                // request to the same service incase any error occured




                HttpResponseMessage response = await _httpClient.GetAsync($"/gateway/Users/GetUserById/{userID}");
                // sending request to API Gateway than to sending it to the user microservice

                if (!response.IsSuccessStatusCode)
                {
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
                        // throw new HttpRequestException($"failed {response.StatusCode}"); 
                        // 
                        // we will send some fix data when it fails

                        return new UserDTO(
                            PersonName: "Temporarily Unavailable",
                            Email: "Temporarily Unavailable",
                            Gender: "Temporarily Unavailable",
                            UserID: Guid.Empty
                            );
                    }


                }


                UserDTO? user = await response.Content.ReadFromJsonAsync<UserDTO>();
                if (user == null)
                {
                    throw new ArgumentException("Invalid User");
                }
                // adding the user to cache so that next 
                // Key:product:{userID}
                //value:{"UserName":"","Email":""} this is how the data is stored in cache

                string userJson = JsonSerializer.Serialize(user);

                //create Cache Options to specify the life to cache data in the application
                DistributedCacheEntryOptions options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(2))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(1));
                string cacheKeyToWrite = $"user:{userID}";
                await _distributedCache.SetStringAsync(cacheKeyToWrite, userJson, options);





                return user;
            }
            catch (BrokenCircuitException ex) {

                _logger.LogError(ex, "Request failed because of circuit breaker in open state , returning dummy data");
                return new UserDTO(
                          PersonName: "Temporarily Unavailable",
                          Email: "Temporarily Unavailable",
                          Gender: "Temporarily Unavailable",
                          UserID: Guid.Empty
                          );


            }
            catch (TimeoutException ex)
            {

                _logger.LogError(ex, "Request failed because of Timeout occurred  returning dummy data");
                return new UserDTO(
                          PersonName: "Temporarily Unavailable",
                          Email: "Temporarily Unavailable",
                          Gender: "Temporarily Unavailable",
                          UserID: Guid.Empty
                          );


            }
        }
    }
}
