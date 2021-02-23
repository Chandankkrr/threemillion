using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GPRCStreaming
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        // GET: /<controller>/
        [HttpGet()]
        public IActionResult Index()
        {
            return Ok("Hello from /home");
        }
    }
}
