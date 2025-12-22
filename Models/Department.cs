namespace Star_Security.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<AppUser> Users { get; set; } = new List<AppUser>();
        public ICollection<SubService> SubServices { get; set; } = new List<SubService>();
        public ICollection<ClientAssignment> ClientAssignments { get; set; } = new List<ClientAssignment>();
        public ICollection<Vacancy> Vacancies { get; set; } = new List<Vacancy>();

    }
}
