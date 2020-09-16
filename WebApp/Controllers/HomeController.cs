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

        public HomeController(ILogger<HomeController> logger, AppDbContext ctx)
        {
            _logger = logger;
            _ctx = ctx;
        }

        public async Task<IActionResult> Index(string sortOrder)
        {

            ViewData["NameSortParam"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParam"] = sortOrder == "date" ? "date_desc" : "date";
            ViewData["FinishedSortParam"] = sortOrder == "finished" ? "finished_desc" : "finished";

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
                case "finished_desc":
                    projects = projects.OrderByDescending(x => x.Finished).ThenBy(x => x.Title);
                    break;
                case "finished":
                    projects = projects.OrderBy(x => x.Finished).ThenBy(x => x.Title);
                    break;
                default:
                    projects = projects.OrderBy(x => x.Title);
                    break;
            }

            var orderedProjects = await projects.AsNoTracking().ToListAsync();
            
            ViewBag.Projects = orderedProjects;
            return View(orderedProjects);
        }

        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}