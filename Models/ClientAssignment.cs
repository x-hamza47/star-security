using System.ComponentModel.DataAnnotations;

namespace Star_Security.Models
{
    public class ClientAssignment
    {
        public int Id { get; set; }

        [Required]
        public int ClientId { get; set; }
        public Client Client { get; set; }

        [Required]
        public int DepartmentId { get; set; }  
        public Department Department { get; set; }

        public string? EmployeeId { get; set; } 
        public AppUser? Employee { get; set; }

        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    }
}
