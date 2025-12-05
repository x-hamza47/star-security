using Microsoft.AspNetCore.Identity;
using System.Diagnostics;

namespace Star_Security.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        public string EmpCode { get; set; }
        public string Address { get; set; }

        public string ContactNumber { get; set; }
        public string EducationalQualification { get; set; }

        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }
        public int? JobRoleId { get; set; }
        public JobRole? JobRole { get; set; }
        public int? GradeId { get; set; }
        public Grade? Grade { get; set; }
        public int? ClientId { get; set; }     
        public Client? Client { get; set; }
        public string? Achievements { get; set; }
        public string? ProfileImage { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}
