using eCommerce.Core.DTO;
using eCommerce.Core.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {
            if (registerRequest == null)
            {
                return BadRequest("invalid data");
            }
            AuthenticationResponse? authenticationResponse = await _userService.Register(registerRequest);
            if (authenticationResponse == null || authenticationResponse.Success == false)
            {
                return BadRequest("register failed");
            }
            return Ok(authenticationResponse);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            if(loginRequest == null)
            {
                return BadRequest("Invalid cred");

            }
            AuthenticationResponse? authenticationResponse = await _userService.Login(loginRequest);
            if (authenticationResponse == null || authenticationResponse.Success == false)
            {
                return Unauthorized("register failed");
            }
            return Ok(authenticationResponse);
        }
    }
}
