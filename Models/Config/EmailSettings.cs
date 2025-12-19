namespace Star_Security.Models.Config
{
    public class EmailSettings
    {
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool EnableSsl { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
    }
}
