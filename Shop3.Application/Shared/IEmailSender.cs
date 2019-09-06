using System.Threading.Tasks;

namespace Shop3.Application.Shared
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
