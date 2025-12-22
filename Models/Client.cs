namespace Star_Security.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Contact { get; set; }
        public string Address { get; set; }
  
        public ICollection<ClientAssignment> ClientAssignments { get; set; } = new List<ClientAssignment>();

    }
}
