using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace WebApp.Models
{
    public class BigProjectForm
    {
        public string Title { get; set; }
        public string Features { get; set; }
        public string Description { get; set; }
        public List<IFormFile> ScreenShots { get; set; }
        public string GithubLink { get; set; }
        public string DemoLink { get; set; }
    }
}