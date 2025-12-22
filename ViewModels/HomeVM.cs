using Star_Security.Models;

namespace Star_Security.ViewModels
{
    public class HomeVM
    {
        public List<Department> Departments { get; set; } = new();
        public List<Region> Regions { get; set; } = new();
        public int TotalDepartments { get; set; }
        public int TotalBranches { get; set; }
    }
}
