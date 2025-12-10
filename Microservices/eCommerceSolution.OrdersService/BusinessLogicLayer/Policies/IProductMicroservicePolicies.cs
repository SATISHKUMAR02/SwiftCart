using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Polly;

namespace BusinessLogicLayer.Policies
{
    public interface IProductMicroservicePolicies
    {
        IAsyncPolicy<HttpResponseMessage> GetFallbackPolicy();
        IAsyncPolicy<HttpResponseMessage> GetBulkHeadPolicy();

        IAsyncPolicy<HttpResponseMessage> GetCombinedPPolicy();
    }
}
