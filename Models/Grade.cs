using System.ComponentModel.DataAnnotations;

namespace Star_Security.Models
{
    public class Grade
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(50)]
        public string LevelCode { get; set; } 

        [StringLength(250)]
        public string Description { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public ICollection<AppUser> Users { get; set; } = new List<AppUser>();
    }
}
