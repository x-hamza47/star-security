namespace Star_Security.ViewModels
{
    public class EditEmployeeVM
    {
        public string Id { get; set; }
        public string EmpCode { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int? DepartmentId { get; set; }
        public int? GradeId { get; set; }
    }
}
