using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Star_Security.ViewModels
{
    public class ClientAssignmentVM
    {
        public int ClientId { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        public string? EmployeeId { get; set; }

        [ValidateNever]
        public List<SelectListItem> Departments { get; set; }
        [ValidateNever]
        public List<SelectListItem> Employees { get; set; }
    }
}
