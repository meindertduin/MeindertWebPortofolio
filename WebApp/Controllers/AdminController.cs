using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly AppDbContext _ctx;
        private const int ITEMS_PER_PAGE = 10;

        public AdminController(AppDbContext ctx)
        {
            _ctx = ctx;
        }
        
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public IActionResult ExerciseProjects(int index)
        {
            var exerciseProjects = _ctx.Projects
                .Where(x => x.Deleted == false)
                .Skip(ITEMS_PER_PAGE * index)
                .Take(ITEMS_PER_PAGE)
                .ToList();
            
            double pagesCount = Math.Ceiling(exerciseProjects.Count() / (double) ITEMS_PER_PAGE);
            
            ViewBag.PagesCount = pagesCount;
            ViewBag.ExerciseProjects = exerciseProjects;
            
            return View();
        }

        [HttpPost, ActionName("CreateExerciseProject")]
        public IActionResult Create(ProjectForm projectForm)
        {
            if (ModelState.IsValid == false)
            {
                return RedirectToAction("ExerciseProjects");
            }
            
            Project newProject = new Project
            {
                Title = projectForm.Title,
                GithubLink = projectForm.GithubLink,
                Description = projectForm.Description,
                TimeAdded = DateTime.Now,
            };


            _ctx.Projects.Add(newProject);
            _ctx.SaveChanges();
            
            return RedirectToAction("ExerciseProjects");
        }

        [HttpPost, ActionName("DeleteExerciseProject")]
        public IActionResult DeleteExerciseProject(int id)
        {
            var project = _ctx.Projects.FirstOrDefault(x => x.Id == id);
            if (project != null)
            {
                project.Deleted = true;
                _ctx.SaveChanges();
            }
            
            return RedirectToAction("ExerciseProjects");
        }

        [HttpGet]
        public IActionResult BigProjects()
        {
            return View();
        }

        [HttpPost, ActionName("CreateBigProject")]
        public IActionResult CreateBigProject(BigProjectForm projectForm)
        {
            List<BigProjectImage> bigProjectImages = new List<BigProjectImage>();
            
            foreach (var screenShot in projectForm.ScreenShots)
            {
                using (var reader = new BinaryReader(screenShot.OpenReadStream()))
                {
                    var bytes = reader.ReadBytes((int) screenShot.Length);
                    
                    bigProjectImages.Add(new BigProjectImage
                    {
                        Image = bytes,
                    });
                }
            }
            
            var bigProject = new BigProject
            {
                Title = projectForm.Title,
                Description = projectForm.Description,
                GithubLink = projectForm.GithubLink,
                Features = projectForm.Features.Split(',', StringSplitOptions.RemoveEmptyEntries).ToArray(),
                Images = bigProjectImages,
            };

            _ctx.BigProjects.Add(bigProject);
            _ctx.SaveChanges();
            
            return RedirectToAction("BigProjects");
        }
    }
}