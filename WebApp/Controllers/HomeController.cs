using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _ctx;
        private int viewPerPage = 10;
        public HomeController(ILogger<HomeController> logger, AppDbContext ctx)
        {
            _logger = logger;
            _ctx = ctx;
        }

        public async Task<IActionResult> Index(string sortOrder, int index)
        {
            var projects = _ctx.Projects.Where(x => x.Deleted == false);

            switch (sortOrder)
            {
                case "name_desc":
                    projects = projects.OrderByDescending(x => x.Title);
                    break;
                case "date":
                    projects = projects.OrderBy(x => x.TimeAdded);
                    break;
                case "date_desc":
                    projects = projects.OrderByDescending(x => x.TimeAdded);
                    break;
                case "name":
                    projects = projects.OrderBy(x => x.Title);
                    break;
                default:
                    projects = projects.OrderByDescending(x => x.TimeAdded);
                    break;
                    
            }
            
            double pagesCount = Math.Ceiling(projects.Count() / (double) viewPerPage);
            var orderedProjects = await projects
                .Skip(viewPerPage * index)
                .Take(viewPerPage)
                .AsNoTracking()
                .ToListAsync();
            
            var bigProjects = _ctx.BigProjects
                .Where(x => x.Deleted == false)
                .Include(x => x.Images)
                .ToList();

            var bigProjectsViewModels = new List<BigProjectViewModel>();

            foreach (var bigProject in bigProjects)
            {
                bigProjectsViewModels.Add(new BigProjectViewModel
                {
                    Id = bigProject.Id,
                    Title = bigProject.Title,
                    Description = bigProject.Description,
                    Features = bigProject.Features,
                    DemoLink = bigProject.DemoLink,
                    GithubLink = bigProject.GithubLink,
                    Images = bigProject.Images.ToList(),
                });
            }
            
            ViewBag.BigProjects = bigProjectsViewModels;
            ViewBag.Projects = orderedProjects;
            ViewBag.PagesCount = pagesCount;
            ViewBag.SortParam = sortOrder;
            
            return View(orderedProjects);
        }
        
        [HttpGet]
        public IActionResult GetBigProjectImage(int id)
        {
            var image = _ctx.BigProjectImages.FirstOrDefault(x => x.Id == id);
            if (image != null)
            {
                return File(image.Image, "image/png");
            }

            return BadRequest();
        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}