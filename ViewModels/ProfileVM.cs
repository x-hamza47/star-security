using System.ComponentModel.DataAnnotations;

namespace Star_Security.ViewModels
{
    public class ProfileVM
    {
        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string Contact { get; set; }

        public string Address { get; set; }

        public string Education { get; set; }

        public IFormFile? ProfileImageFile { get; set; }

        public string? ExistingProfileImage { get; set; }

        public string? DepartmentName { get; set; }
        public string? GradeName { get; set; }
        public string? ClientName { get; set; }
    }
}
