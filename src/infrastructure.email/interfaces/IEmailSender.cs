using System.Threading.Tasks;

namespace infrastructure.email.interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
