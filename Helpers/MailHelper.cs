using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;

namespace XpenseTracker.Helpers
{
    public class MailHelper
    {
        private readonly IConfiguration configuration;

        public MailHelper(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail,string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(configuration["MailSettings:SenderEmail"]));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = body };

            using (var smtp = new MailKit.Net.Smtp.SmtpClient())
            {
                await smtp.ConnectAsync(configuration["MailSettings:SmtpServer"], int.Parse(configuration["MailSettings:Port"]), false);
                await smtp.AuthenticateAsync(configuration["MailSettings:Username"], configuration["MailSettings:Password"]);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
        }




        
    }
}