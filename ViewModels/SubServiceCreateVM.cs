using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Star_Security.ViewModels
{
    public class SubServiceCreateVM
    {
        [Required(ErrorMessage = "SubService Name is required")]
        [StringLength(100)]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Department is required")]
        public int DepartmentId { get; set; }

        public IEnumerable<SelectListItem> Departments { get; set; } = new List<SelectListItem>();
    }
}
