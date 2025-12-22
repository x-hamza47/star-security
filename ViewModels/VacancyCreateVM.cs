using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Star_Security.ViewModels
{
    public class VacancyCreateVM
    {
        [Required, StringLength(150)]
        public string Title { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        public int? SubServiceId { get; set; }

        [Required]
        public int GradeId { get; set; }

        [Required]
        public int RequiredStaff { get; set; }

        public DateTime LastDate { get; set; } = DateTime.Today.AddDays(7);

        public bool IsActive { get; set; } = true;

        [ValidateNever]
        public List<SelectListItem> Departments { get; set; }

        [ValidateNever]
        public List<SelectListItem> SubServices { get; set; }

        [ValidateNever]
        public List<SelectListItem> Grades { get; set; } 
    }


}
