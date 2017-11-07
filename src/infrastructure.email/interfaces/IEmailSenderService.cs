using System.Threading.Tasks;

namespace infrastructure.email.interfaces
{
    public interface IEmailSenderService
    {
        Task sendEmailAsync(string emailTo, string emailFrom, string emailFromName, string subject, string body);
    }
}
