using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CompClubAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class Pages : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "pages", "index.html"), "text/html");
        }

        [HttpGet("{page}")]
        public IActionResult Page(string page)
        {
            return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "pages", $"{page}.html"), "text/html");
        }
    }
}
