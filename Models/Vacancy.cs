using System.ComponentModel.DataAnnotations;

namespace Star_Security.Models
{
    public class Vacancy
    {
        public int Id { get; set; }

        // Job role
        [Required, StringLength(150)]
        public string Title { get; set; }

        [Required]
        public int DepartmentId { get; set; } 
        public Department Department { get; set; }

        public int? SubServiceId { get; set; }   
        public SubService SubService { get; set; }

        [Required]
        public int GradeId { get; set; }
        public Grade Grade { get; set; }

        [Required]
        public int RequiredStaff { get; set; }

        public int FilledStaff { get; set; } = 0;

        public DateTime LastDate { get; set; }
        public bool IsActive { get; set; } = true;
  
        public ICollection<VacancyApplication> Applications { get; set; }
    }
}
