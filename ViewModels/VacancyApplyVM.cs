using System.ComponentModel.DataAnnotations;

namespace Star_Security.ViewModels
{
    public class VacancyApplyVM
    {
        [Required]
        public int VacancyId { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string ContactNumber { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public string? Education { get; set; }
        public string? Address { get; set; }
    }
}
