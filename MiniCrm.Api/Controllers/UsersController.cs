using Microsoft.AspNetCore.Mvc;

namespace MiniCrm.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UsersController : ControllerBase
    {


        private readonly ILogger<UsersController> _logger;

        public UsersController(ILogger<UsersController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await Task.FromResult(0);
            return Ok();
        }
    }
}
