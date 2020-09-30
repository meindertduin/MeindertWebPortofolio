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
            var orderedProjects = await projects.Skip(index != null? viewPerPage * index: 0)
                .Take(viewPerPage)
                .AsNoTracking()
                .ToListAsync();
            
            ViewBag.Projects = orderedProjects;
            ViewBag.PagesCount = pagesCount;
            ViewBag.SortParam = sortOrder;
            
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