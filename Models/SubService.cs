using System.ComponentModel.DataAnnotations;

namespace Star_Security.Models
{
    public class SubService
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string? Description { get; set; }

        public int DepartmentId { get; set; }   
        public Department Department { get; set; }

        public ICollection<Vacancy> Vacancies { get; set; } = new List<Vacancy>();
    }
}
