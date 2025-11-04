using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinesslogicLayer.DTO
{
    public record ProductResponse(Guid ProductID , string ProductName,CategoryOptions Category,
        double? UnitPrice , int? Quantity)
    {
        public ProductResponse() : this(default, default, default, default, default)
        {

        }
    }
    
}
