using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;


namespace Shop3.Application.Shared
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //triển khai send mail
        public Task SendEmailAsync(string email, string subject, string message)
        {
            //create smtp get thông tin trên server( appsettings.json ) 
            SmtpClient client = new SmtpClient(_configuration["MailSettings:Server"])
            {
                UseDefaultCredentials = false,
                Port = int.Parse(_configuration["MailSettings:Port"]),
                EnableSsl = bool.Parse(_configuration["MailSettings:EnableSsl"]),
                Credentials = new NetworkCredential(_configuration["MailSettings:UserName"], _configuration["MailSettings:Password"])
            };
            // create MailMessage form user
            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["MailSettings:FromEmail"],
                    _configuration["MailSettings:FromName"]),
            };

            mailMessage.To.Add(email);
            mailMessage.Body = message;
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true; // cho phép send html
            client.Send(mailMessage);
            return Task.CompletedTask;
        }
    }
}
