using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCommerce.Core.DTO;

namespace eCommerce.Core.ServiceContracts
{
    public interface IUserService
    {
       Task<AuthenticationResponse?> Login(LoginRequest loginRequest); 
       Task<AuthenticationResponse?> Register(RegisterRequest registerRequest);
    }
}
