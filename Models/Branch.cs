using System.ComponentModel.DataAnnotations;

namespace Star_Security.Models
{
    public class Branch
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }     

        [StringLength(150)]
        public string Area { get; set; }  

        [StringLength(100)]
        public string ManagerName { get; set; }

        [Phone, StringLength(20)]
        public string ContactNumber { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public bool IsActive { get; set; } = true;

        public int RegionId { get; set; }
        public Region Region { get; set; }
    }
}
