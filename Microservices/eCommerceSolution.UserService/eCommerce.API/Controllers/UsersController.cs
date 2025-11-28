using eCommerce.Core.DTO;
using eCommerce.Core.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        [Route("GetUserByID/{UserID:guid}")]
        public async Task<IActionResult> GetUserByUserID(Guid UserID)
        {
            if (UserID == Guid.Empty)
            {
                return BadRequest();
            }
            UserDTO? response = await _userService.GetUserByUserId(UserID);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("GetAllUsers")]

        public async Task<IActionResult> GetAllUsers()
        {
            List<UserDTO> response = await _userService.GetAllUsers();
            return Ok(response.ToList());
        }
    }

}
