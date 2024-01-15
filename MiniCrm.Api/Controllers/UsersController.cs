using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using MiniCrm.Core.Contracts.Users;
using MiniCrm.Core.Interfaces;

namespace MiniCrm.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UsersController : ControllerBase
    {

        private readonly ILogger<UsersController> _logger;
        private readonly IUserService _userService;

        public UsersController(ILogger<UsersController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpGet("id")]
        public async Task<IActionResult> Get([FromRoute] string id)
        {
            var response = await _userService.Get(HttpContext.RequestAborted);
            return Ok(response);
        }
        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            var response = await _userService.Get(HttpContext.RequestAborted);
            return Ok(response);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login()
        {
            await Task.FromResult(0);
            return Ok();
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserRequest userRequest)
        {
            var response = await _userService.Register(userRequest, HttpContext.RequestAborted);
            return Ok(response);
        }
        [HttpPost("Change-Passord")]
        public async Task<IActionResult> ChangePassword()
        {
            await Task.FromResult(0);
            return Ok();
        }

        [HttpPost("Forget-Passord")]
        public async Task<IActionResult> ForgetPassword()
        {
            await Task.FromResult(0);
            return Ok();
        }
        [HttpPost("Update-profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UserRequest userRequest)
        {
            var response = await _userService.Register(userRequest, HttpContext.RequestAborted);
            return Ok(response);
        }
    }
}
