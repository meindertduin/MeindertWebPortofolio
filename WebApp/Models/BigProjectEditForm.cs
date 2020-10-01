using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace WebApp.Models
{
    public class BigProjectEditForm
    {
        public int Id { get; set; }
        
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public string Features { get; set; }
        
        public string GithubLink { get; set; }

        public List<IFormFile> ScreenShots { get; set; }
    }
}