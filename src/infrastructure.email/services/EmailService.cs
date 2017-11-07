using infrastructure.email.interfaces;
using infrastructure.email.models;
using infrastructure.email.repository;
using infrastructure.libs.validators;
using System;

namespace infrastructure.email.services
{
    public class EmailService : IEmailService
    {   
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly IEmailSenderService _emailSenderService;

        public EmailService(
               IEmailTemplateService emailTemplateService,
               IEmailSenderService emailSenderService
               )
        {
            _emailTemplateService = emailTemplateService;
            _emailSenderService = emailSenderService;
        }

        public void sendPasswordReminder(string emailTo, string link, DateTime expiryDate)
        {
            CustomValidators.StringNotNullorEmpty(emailTo, "emailTo is null or empty");
            CustomValidators.StringNotNullorEmpty(link, "link is null or empty");
            CustomValidators.NotNull(expiryDate, "expiryDate is null");

           // get Email Template and set it up 
            var emailToSend = _emailTemplateService.GetPasswordReminderTemplate( emailTo, link, expiryDate);
            _emailSenderService.sendEmailAsync(emailToSend.Receiver, emailToSend.Sender, emailToSend.SenderName, emailToSend.Subject, emailToSend.Body);
        }

        public void sendEmailChangeActivation(string emailTo, string link, DateTime expiryDate)
        {
            CustomValidators.StringNotNullorEmpty(emailTo, "emailTo is null or empty");
            CustomValidators.StringNotNullorEmpty(link, "link is null or empty");
            CustomValidators.NotNull(expiryDate, "expiryDate is null");

            // get Email Template and set it up 
            var emailToSend = _emailTemplateService.GetEmailChangeActivationTemplate(emailTo, link, expiryDate);
            _emailSenderService.sendEmailAsync(emailToSend.Receiver, emailToSend.Sender, emailToSend.SenderName, emailToSend.Subject, emailToSend.Body);
        }
      
    }



}
