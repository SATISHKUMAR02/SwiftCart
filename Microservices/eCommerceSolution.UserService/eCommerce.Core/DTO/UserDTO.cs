using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.DTO
{
    // this is for the Microservice Communitcation with the other services for getting the user Id
    // syn method
    public record UserDTO(Guid UserID,string? Email,string? PersonName , string? Gender);
    
    
}
