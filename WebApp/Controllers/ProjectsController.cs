using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Route("api/projects")]
    public class ProjectsController : Controller    
    {
        private readonly AppDbContext _ctx;

        public ProjectsController(AppDbContext ctx)
        {
            _ctx = ctx;
        }
        
        [HttpGet("exercise")]
        public IActionResult Index()
        {

            return View();
        }
        
        [HttpPost("exercise")]
        public async Task<IActionResult> Index(ProjectForm projectForm)
        {
            if (ModelState.IsValid == false)
            {
                ViewBag.Message = "Sorry maar de aanvraag kan niet verwerkt worden";
                return View();
            }

            try
            {
                Project newProject = new Project
                {
                    Title = projectForm.Title,
                    Description = projectForm.Description,
                    TimeAdded = DateTime.Now,
                };

                await _ctx.Projects.AddAsync(newProject);
                var res = await _ctx.SaveChangesAsync();

                ViewBag.Message = "gelukt!";
            }
            catch (Exception e)
            {
                ViewBag.Message = "Sorry maar de aanvraag kan niet verwerkt worden";
            }

            return View();
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