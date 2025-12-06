using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DnsClient.Internal;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Timeout;
using Polly.Wrap;

namespace BusinessLogicLayer.Policies
{
    public class UsersMicroservicePolicies : IUsersMicroservicePolicies
    {
        private readonly ILogger<UsersMicroservicePolicies> _logger;
        public UsersMicroservicePolicies(ILogger<UsersMicroservicePolicies> logger)
        {
            _logger = logger;
        }
        public IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
           AsyncRetryPolicy<HttpResponseMessage> policy = Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode).WaitAndRetryAsync(retryCount:3 ,
            sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,retryAttempt)),

            // math.Pow is done to prevent port exhaustion 
            // this code executes automatically just before making a retry request
            onRetry: (outcome, timespan, retryAttempt, context) =>
            {
                // the retry policy in Polly is suitable for transient errors that is temperory errors like server down , exceptions etc

                _logger.LogInformation($"Retry {retryAttempt} after {timespan.TotalSeconds} seconds");
            
            }
            );
            return policy;
        }

        public IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            AsyncCircuitBreakerPolicy<HttpResponseMessage> policy = Policy

                .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)

                .CircuitBreakerAsync(
                    handledEventsAllowedBeforeBreaking: 3, durationOfBreak: TimeSpan.FromMinutes(2),
                    onBreak: (outcome, timespan) =>
                    {
                        _logger.LogInformation(
                            $"Circuit breaker is now open and requests are blocked for {timespan.TotalMinutes} minutes");
                    },
                    onReset: () =>
                    {
                        _logger.LogInformation("Circuit breaker is closed, requests will be allowed");
                    });

            return policy;
        }

        public IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy()
        {
            AsyncTimeoutPolicy<HttpResponseMessage> policy = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromMilliseconds(1500));
            return policy;

        }

        public IAsyncPolicy<HttpResponseMessage> GetCombinedPolicy()
        {
            var retryPolicy = GetRetryPolicy();
            var circuitBreakerPolicy = GetCircuitBreakerPolicy();
            var timeoutPolicy = GetTimeoutPolicy();
            AsyncPolicyWrap<HttpResponseMessage> policies =  Policy.WrapAsync(retryPolicy,circuitBreakerPolicy, timeoutPolicy);
            return policies;
        }
    }
}
