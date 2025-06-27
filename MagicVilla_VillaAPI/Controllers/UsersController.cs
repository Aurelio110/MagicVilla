using MagicVilla_VillaAPI.Repo.IRepo;
using MagicVilla_VillaAPI.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/UserAuth")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUserRepo _userRepo;
      
        public UsersController(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            var response = await _userRepo.Login(model);
            if (response.User == null || string.IsNullOrEmpty((string?)response.Token))
            {
                return BadRequest("Invalid login details.");
            }
            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest model)
        {
            if (model == null || string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest("Invalid registration details.");
            }
            if (!_userRepo.IsUniqueUser(model.UserName))
            {
                return BadRequest("User already exists.");
            }
            var response = await _userRepo.Register(model);
            if (response == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error registering user.");
            }
            return Ok(response);
        }
    }
}
