using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Star_Security.Models
{
    public class VacancyApplication
    {
        public int Id { get; set; }

        public int VacancyId { get; set; }
        public Vacancy Vacancy { get; set; }


        [Required] public string FullName { get; set; }
        [Required] public string ContactNumber { get; set; }
        [Required, EmailAddress] public string Email { get; set; }

        public string Education { get; set; }
        public string Address { get; set; }

        [ValidateNever]
        public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;
        [ValidateNever]
        public DateTime AppliedAt { get; set; } = DateTime.Now;
    }

    public enum ApplicationStatus
    {
        Pending,
        Approved,
        Rejected
    }
}
