namespace Star_Security.ViewModels
{
    public class EmployeeListVM
    {
        public string Id { get; set; }
        public string EmpCode { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }

        public int? DepartmentId { get; set; }
        public string Department { get; set; }

        public int? GradeId { get; set; }
        public string Grade { get; set; }

        public int? ClientId { get; set; }
        public string Client { get; set; }

        public string Achievements { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
