using System;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(300)]
        public string GithubLink { get; set; }
        [MaxLength(50)]
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Deleted { get; set; }
        public DateTime TimeAdded { get; set; }
    }
}