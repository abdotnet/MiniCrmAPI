using Microsoft.AspNetCore.Mvc;

namespace MiniCrm.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CustomerController
    : ControllerBase
    {


        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ILogger<CustomerController> logger)
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
