using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class BigProject
    {
        [Key]
        public int Id { get; set; }
        
        [MaxLength(50)]
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public string[] Features { get; set; }
        
        [MaxLength(300)]
        public string GithubLink { get; set; }

        public bool Deleted { get; set; }

        public ICollection<BigProjectImage> Images { get; set; }
    }
}