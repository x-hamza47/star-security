using System.ComponentModel.DataAnnotations;

namespace Star_Security.Models
{
    public class Region
    {
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; } 

        public ICollection<Branch> Branches { get; set; } = new List<Branch>();
    }
}
