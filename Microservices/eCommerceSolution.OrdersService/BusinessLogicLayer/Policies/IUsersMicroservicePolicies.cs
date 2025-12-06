using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Polly;

namespace BusinessLogicLayer.Policies
{
    public interface IUsersMicroservicePolicies
    {
        IAsyncPolicy<HttpResponseMessage> GetRetryPolicy();
        IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy();
        IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy();
        IAsyncPolicy<HttpResponseMessage> GetCombinedPolicy();



    }
}
