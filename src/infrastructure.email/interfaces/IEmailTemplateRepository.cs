
using infrastructure.email.models;
namespace infrastructure.email.repository
{
    public interface IEmailTemplateRepository
    {
        EmailTemplate getTemplateByType(string templateTypeName);
    }
}
