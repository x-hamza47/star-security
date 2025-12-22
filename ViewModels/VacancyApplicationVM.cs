using System.ComponentModel.DataAnnotations;

namespace Star_Security.ViewModels
{
    public class VacancyApplicationVM
    {
        public int VacancyId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required, EmailAddress] 
        public string Email { get; set; }
        [Required] 
        public string Contact { get; set; }
    }
}
