
using infrastructure.email.entities;

namespace infrastructure.email.repository
{
    public interface IEmailTemplateRepository
    {
        EmailTemplateDb getTemplateByType(string templateTypeName);
    }
}
