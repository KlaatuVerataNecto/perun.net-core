using System;
using infrastructure.email.interfaces;
using infrastructure.libs.validators;
using infrastructure.email.repository;
using infrastructure.email.models;

namespace infrastructure.email.services
{
    public class EmailTemplateService : IEmailTemplateService
    {
        private const string RESEND_PASSWORD_TEMPLATE = "resend_password";

        private readonly IEmailTemplateRepository _emailTemplateRepository;

        public EmailTemplateService(IEmailTemplateRepository emailTemplateRepository)
        {
            _emailTemplateRepository = emailTemplateRepository;
        }

        public Email GetPasswordReminderTemplate(string emailTo,string token, DateTime expiryDate)
        {
            CustomValidators.StringNotNullorEmpty(token, "token string is null or empty.");
            CustomValidators.DateTimeIsInFuture(expiryDate, "expiryDate is not a future date.");

            var emailTemplate = _emailTemplateRepository.getTemplateByType(RESEND_PASSWORD_TEMPLATE);

            CustomValidators.NotNull(emailTemplate, "emailTemplate is null or empty.");

            var body = emailTemplate.email_body.Replace("{link}", token);
            return new Email(emailTemplate.sender_email,emailTemplate.sender_name ,emailTo, emailTemplate.email_subject, body);
        }
    }
}
