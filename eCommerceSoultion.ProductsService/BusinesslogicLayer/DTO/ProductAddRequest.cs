using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinesslogicLayer.DTO
{
    public record ProductAddRequest(string ProductName, CategoryOptions CategoryOptions,
        double? UnitPrice, int? quantity)
    {
        public ProductAddRequest() : this( default, default, default, default)
        {

        }
    }
   
}
