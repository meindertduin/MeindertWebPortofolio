using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/projects")]
    public class ProjectsController : ControllerBase    
    {
        [HttpGet("exercise")]
        public string Get()
        {
            
            return "test";
        }
        
        [HttpPost("exercise")]
        public IActionResult Create()
        {
            return Ok();
        }
        
        [HttpDelete("exercise")]
        public IActionResult Delete()
        {
            return Ok();
        }
        
        [HttpPut("exercise")]
        public IActionResult Change()
        {
            return Ok();
        }
    }
}