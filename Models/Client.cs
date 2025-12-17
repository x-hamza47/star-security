namespace Star_Security.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Contact { get; set; }
        public string Address { get; set; }
        public ICollection<AppUser> Users { get; set; } = new List<AppUser>();
    }
}
