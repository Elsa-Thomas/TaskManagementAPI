using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Model;
using TaskManagement.Services;
using TaskManagement.Services.Interfaces;
using System.Threading.Tasks;

namespace TaskManagement.Controllers
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

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto request)
        {
            var response = await _userService.RegisterUserAsync(request);
            if (!response.Success)
                return BadRequest(response.Message);

            return Ok(response.Data);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto request)
        {
            var response = await _userService.AuthenticateUserAsync(request);
            if (!response.Success)
                return Unauthorized(response.Message);

            return Ok(response.Data);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = await _userService.GetAllUsersAsync();
            return Ok(response.Data);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var response = await _userService.GetUserByIdAsync(id);
            if (!response.Success)
                return NotFound(response.Message);

            return Ok(response.Data);
        }
    }
}






