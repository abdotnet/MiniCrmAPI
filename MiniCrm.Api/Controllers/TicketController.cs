using Microsoft.AspNetCore.Mvc;

namespace MiniCrm.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TicketController : ControllerBase
    {


        private readonly ILogger<TicketController> _logger;

        public TicketController(ILogger<TicketController> logger)
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
