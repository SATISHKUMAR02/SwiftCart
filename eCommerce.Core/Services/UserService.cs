using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using eCommerce.Core.DTO;
using eCommerce.Core.Entities;
using eCommerce.Core.RepositoryContracts;
using eCommerce.Core.ServiceContracts;

namespace eCommerce.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserService(IUserRepository userRepository,IMapper mapper)
        {
            _userRepository = userRepository;   
            _mapper = mapper;
        }
        public async Task<AuthenticationResponse?> Login(LoginRequest loginRequest)
        {
            ApplicationUser user = await _userRepository.GetUserByEmailAndPassword(loginRequest.Email, loginRequest.Password);
            if (user == null) {
                return null;
            }
            //return new AuthenticationResponse(user.UserId, user.Email, user.PersonName,user.Gender,"token",Success : true);
            return _mapper.Map<AuthenticationResponse>(user) with
             {
                 Success = true, Token = "Token"
             };


        }

        public async Task<AuthenticationResponse?> Register(RegisterRequest registerRequest)
        {
            ApplicationUser user = new ApplicationUser()
            {
                PersonName = registerRequest.PersonName,
                Email = registerRequest.Email,
                Password = registerRequest.Password,
                Gender = registerRequest.gender.ToString(),
            };
            ApplicationUser? registered = await _userRepository.AddUser(user);
            if (registered == null) {
                return null;
            }
            //return new AuthenticationResponse(registered.UserId, 
            //    registered.Email, 
            //    registered.PersonName,
            //    registered.Gender,
            //    "Token",Success:true);
            return _mapper.Map<AuthenticationResponse>(registered) with
            {
                Success = true,
                Token = "Token"
            };
        }
    }
}
