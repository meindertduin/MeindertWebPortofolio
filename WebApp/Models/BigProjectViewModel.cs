using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace WebApp.Models
{
    public class BigProjectViewModel
    {
        public int Id { get; set; }
        
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public string[] Features { get; set; }
        
        public string GithubLink { get; set; }

        public List<BigProjectImage> Images { get; set; }
    }
}