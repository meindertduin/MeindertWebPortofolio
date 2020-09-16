using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class ContactController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}