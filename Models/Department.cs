using System.ComponentModel.DataAnnotations;

namespace Star_Security.Models
{
    public class Department
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Department Name is required")]
        [StringLength(50, ErrorMessage = "Name can't exceed 50 characters")]
        public string Name { get; set; }

        [Required]
        [StringLength(10)]
        public string Code { get; set; } 

        [StringLength(250)]
        public string Description { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public ICollection<AppUser> Users { get; set; } = new List<AppUser>();

    }
}
