using System.Net;
using System.Net.Mail;

namespace Star_Security.Services
{
    public class EmailSender
    {
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _username;
        private readonly string _password;
        private readonly bool _enableSsl;
        private readonly string _senderName;
        private readonly string _senderEmail;

        // Load SMTP config from appsettings.json
        public EmailSender(IConfiguration configuration)
        {
            var section = configuration.GetSection("EmailSettings");
            _smtpHost = section["SmtpHost"];
            _smtpPort = int.Parse(section["SmtpPort"]);
            _username = section["Username"];
            _password = section["Password"];
            _enableSsl = bool.Parse(section["EnableSsl"] ?? "true");
            _senderName = section["SenderName"];
            _senderEmail = section["SenderEmail"];
        }

        // Only need to pass recipient, subject, body
        public async Task SendMailAsync(string toEmail, string subject, string body)
        {
            using var mail = new MailMessage();
            mail.From = new MailAddress(_senderEmail, _senderName);
            mail.To.Add(toEmail);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            using var smtp = new SmtpClient(_smtpHost, _smtpPort)
            {
                Credentials = new NetworkCredential(_username, _password),
                EnableSsl = _enableSsl
            };

            await smtp.SendMailAsync(mail);
        }
    }
}
