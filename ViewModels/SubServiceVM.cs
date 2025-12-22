using System.ComponentModel.DataAnnotations;

namespace Star_Security.ViewModels
{
    public class SubServiceVM
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required]
        public int DepartmentId { get; set; }
    }
}
