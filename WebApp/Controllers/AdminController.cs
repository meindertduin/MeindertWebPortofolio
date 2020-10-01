using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly AppDbContext _ctx;
        private readonly IWebHostEnvironment _env;
        private const int ITEMS_PER_PAGE = 10;

        private const int BIG_PROJECT_IMAGE_WIDTH = 1280;
        private const int BIG_PROJECT_IMAGE_HEIGHT = 720;
        
        public AdminController(AppDbContext ctx, IWebHostEnvironment env)
        {
            _ctx = ctx;
            _env = env;
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
                    GithubLink = bigProject.GithubLink,
                    Images = bigProject.Images.ToList(),
                });
            }


            ViewBag.BigProjects = bigProjectsViewModels;
            
            return View();
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
        
        [HttpPost, ActionName("CreateBigProject")]
        public async Task<IActionResult> CreateBigProject(BigProjectForm projectForm)
        {
            List<BigProjectImage> bigProjectImages = new List<BigProjectImage>();
            
            foreach (var screenShot in projectForm.ScreenShots)
            {
                using (var input = screenShot.OpenReadStream())
                {
                    var tempSavePath = Path.Combine(_env.WebRootPath, Path.GetRandomFileName() + ".jpg");
                    
                    using (var image = Image.Load(input))
                    {
                        image.Mutate(x => 
                            x.Resize(new Size(BIG_PROJECT_IMAGE_WIDTH, BIG_PROJECT_IMAGE_HEIGHT)));
                            
                        await image.SaveAsync(tempSavePath, new JpegEncoder());
                    }
                        
                    using (var reader = new BinaryReader(System.IO.File.OpenRead(tempSavePath)))
                    {
                        var bytes = reader.ReadBytes((int) screenShot.Length);
                    
                        bigProjectImages.Add(new BigProjectImage
                        {
                            Image = bytes,
                        });
                    }
                    
                    System.IO.File.Delete(tempSavePath);
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