using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class BigProjectImage
    {
        [Key]
        public int Id { get; set; }

        public byte[] Image { get; set; }
        
        public int BigProjectId { get; set; }
        public BigProject BigProject { get; set; }
    }
}