using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Star_Security.ViewModels
{
    public class CreateEmployeeVM
    {
        [Display(Name = "Employee Code")]
        public string EmpCode { get; set; } 

        [Required(ErrorMessage = "Employee Name is required")]
        [StringLength(100, ErrorMessage = "Name can't exceed 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(250, ErrorMessage = "Address can't exceed 250 characters")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Contact number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string Contact { get; set; }

        [Required(ErrorMessage = "Education is required")]
        [StringLength(100, ErrorMessage = "Education can't exceed 100 characters")]
        public string Education { get; set; }

        [Required(ErrorMessage = "Department is required")]
        [Display(Name = "Department")]
        public int? DepartmentId { get; set; }

        [Required(ErrorMessage = "Grade is required")]
        [Display(Name = "Grade")]
        public int? GradeId { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> Departments { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> Grades { get; set; }
    }
}
