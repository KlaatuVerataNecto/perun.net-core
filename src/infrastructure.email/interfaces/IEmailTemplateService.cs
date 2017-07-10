using infrastructure.email.models;
using System;

namespace infrastructure.email.interfaces
{
    public interface IEmailTemplateService
    {
        Email GetPasswordReminderTemplate(string emailTo, string token, DateTime expiryDate);
    }
}
