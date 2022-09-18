using HotelListing.API.Core.Interfaces;
using HotelListing.API.Core.Dtos.UserDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HotelListing.API.Data.Constants;

namespace HotelListing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAuthManager _manager;
        private readonly ILogger<UserController> _logger;

        public UserController(IAuthManager manager, ILogger<UserController> logger)
        {
            _manager = manager;
            _logger = logger;
        }

        [HttpPost]
        [Route("register")] // api/User/Register
        [ProducesResponseType(StatusCodes.Status400BadRequest)] //tell controller it's possible to have 400 code, it construct a default API response for exception
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Register([FromBody] UserDto userDto)
        {
            _logger.LogInformation($"Registration attempt for {userDto.Email}");
            var errors = await _manager.Register(userDto);

            if (errors.Any())
            {
                 foreach (var error in errors)
                 {
                     ModelState.AddModelError(error.Code, error.Description);
                 }
                 return BadRequest(ModelState);
            }

            return Ok();
        }

        [HttpPost]
        [Route("login")] // api/User/login
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
        {
            _logger.LogInformation($"Login attempt from {loginDto.Email}");
            var authResponse = await _manager.Login(loginDto);
            if (authResponse is null)
            {
                return BadRequest();
            }
            return Ok(authResponse);
           
        }

        [HttpPost]
        [Route("refresh-token")] // api/User/refresh-token
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> RefreshToken(AuthResponseDto authResponseDto)
        {
            var authResponse = await _manager.VerifyRefreshToken(authResponseDto);
            if (authResponse is null) return Unauthorized();
            return Ok(authResponse);
        }

        [HttpPost]
        [Route("register-admin")] // api/User/register-admin
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = Const.Roles.Administrator)]
        public async Task<ActionResult> RegisterAdmin(UserDto userDto)
        {
            var errors = await _manager.RegisterAdmin(userDto);
            if(errors.Any())
            {
                foreach(var error in errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return Unauthorized(ModelState);
            }
            return Ok();
        }
    }
}
