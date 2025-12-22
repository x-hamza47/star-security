using Microsoft.AspNetCore.Identity;
using System.Diagnostics;

namespace Star_Security.Models
{
    public class AppUser : IdentityUser
    {
        public string EmpCode { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public string Contact { get; set; }
        public string Education { get; set; }

        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }
        public int? GradeId { get; set; }
        public Grade? Grade { get; set; }
        public int? ClientId { get; set; }     
        public Client? Client { get; set; }
        public string? Achievements { get; set; }
        public string? ProfileImage { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int? HiredFromVacancyId { get; set; }
        public Vacancy? HiredFromVacancy { get; set; }
        public ICollection<ClientAssignment> ClientAssignments { get; set; } = new List<ClientAssignment>();

    }
}
