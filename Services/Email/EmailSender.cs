using Microsoft.Extensions.Options;
using Star_Security.Models.Config;
using System.Net;
using System.Net.Mail;

namespace Star_Security.Services.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings settings;

        public EmailSender(IOptions<EmailSettings> options)
        {
            settings = options.Value;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var mail = new MailMessage
            {
               From = new MailAddress(
                   settings.SenderEmail,
                   settings.SenderName
               ),
               Subject = subject,
               Body = body,
               IsBodyHtml = true
            };

            mail.To.Add(to);
             using var smtp = new SmtpClient(
                 settings.SmtpHost,
                 settings.SmtpPort
             )
             {
                 Credentials = new NetworkCredential(
                     settings.Username,
                     settings.Password
                 ),
                 EnableSsl = settings.EnableSsl,
             };

            await smtp.SendMailAsync( mail );
        }
    }
}
