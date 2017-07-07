using infrastructure.email.interfaces;
using infrastructure.email.models;
using infrastructure.email.repository;
using infrastructure.libs.validators;
using System;

namespace infrastructure.email.services
{



    public class EmailService : IEmailService
    {
        private readonly IEmailSettingsService _emailSettingsService;
        private readonly IEmailRepository _emailRepository;
        private readonly IEmailTemplateRepository _emailTemplateRepository;
        private readonly IEmailTemplateService _emailTemplateService;

        private const string RESEND_PASSWORD_TEMPLATE = "resend_password";

        public EmailService(
               IEmailRepository emailRepository, 
               IEmailTemplateRepository emailTemplateRepository,
               IEmailSettingsService emailSettingsService,
               IEmailTemplateService emailTemplateService
               )
        {
            _emailRepository = emailRepository;
            _emailTemplateRepository = emailTemplateRepository;
            _emailSettingsService = emailSettingsService;
            _emailTemplateService = emailTemplateService;
        }

        public bool sendPasswordReminder(string emailTo, string token, DateTime expiryDate)
        {
            CustomValidators.StringNotNullorEmpty(emailTo, "emailTo is null or empty");
            CustomValidators.StringNotNullorEmpty(token, "token is null or empty");
            CustomValidators.NotNull(expiryDate, "expiryDate is null");

            //var emailTemplate = _emailTemplateRepository.getTemplateByType(RESEND_PASSWORD_TEMPLATE);
            //var emailToSend = _emailTemplateService.SetPasswordReminderTemplate(
            //    emailTemplate, emailTo, token, expiryDate
            //);

            throw new NotImplementedException();

            /* TODO: email template check */

            //var mailer = new AsyncEmailer();
            //mail.to = email.Receiver;
            //mail.subject = email.Subject;
            //mail.body = email.Body;
            //mailer.SendThat(mail);

            //email.EmailSent();
            //var saved_email = _emailRepository.Add(email);

            //return (saved_email != null);
        }

        //public bool TEMPsendPasswordReminder(MailMessage mail, string url)
        //{
        //    // Clean Database First
        //    CustomValidators.NotNull(mail, "mail string is null or empty");
        //    var email = get_template_with_url(RESEND_PASSWORD_TEMPLATE, url, mail.to);

        //    /* TODO: email template check */

        //    var mailer = new AsyncEmailer();
        //    mail.to = email.Receiver;
        //    mail.subject = email.Subject;
        //    mail.body = email.Body;
        //    mailer.SendThat(mail);

        //    email.EmailSent();
        //    var saved_email = _emailRepository.Add(email);

        //    return (saved_email != null);
        //}

        //public bool sendChangeEmail(MailMessage mail, string url)
        //{
        //    CustomValidators.NotNull(mail, "mail string is null or empty");



        //    var email = get_template_with_url("change_email", url, mail.to);
        //    var mailer = new AsyncEmailer();
        //    mail.to = email.Receiver;
        //    mail.subject = email.Subject;
        //    mail.body = email.Body;
        //    mailer.SendThat(mail);
        //    email.EmailSent();
        //    var saved_email = _emailRepository.Add(email);
        //    return (saved_email != null);
        //}

        //public bool sendUserAccountInfo(MailMessage mail, string username, string password /*, string firstname, string lastname*/)
        //{
        //    CustomValidators.NotNull(mail, "mail string is null or empty");



        //    var email = get_template_with_password("send_password", username, password, mail.to /*firstname, lastname*/);
        //    var mailer = new AsyncEmailer();
        //    mail.to = email.Receiver;
        //    mail.subject = email.Subject;
        //    mail.body = email.Body;
        //    mailer.SendThat(mail);
        //    email.EmailSent();
        //    var saved_email = _emailRepository.Add(email);
        //    return (saved_email != null);
        //}

        //private Email get_template_with_url(string template_name, string url, string to)
        //{

        //    CustomValidators.StringNotNullorEmpty(template_name, "template_name string is null or empty");
        //    CustomValidators.StringNotNullorEmpty(url, "url string is null or empty");
        //    CustomValidators.StringNotNullorEmpty(to, "'to' string is null or empty");

        //    var email_template = _emailTemplateRepository.get_template_by_type(template_name);

        //    if (email_template == null)
        //        return null;


        //    var body = email_template.Body;

        //    if (url != null)
        //    {
        //        body = body.Replace("{link}", url);
        //    }
        //    return new Email(email_template.Sender, to, email_template.Subject, body);
        //}

        //private Email get_template_with_password(string template_name, string username, string password, string to /* string firstname, string lastname*/)
        //{
        //    CustomValidators.StringNotNullorEmpty(template_name, "template_name string is null or empty");
        //    /*CustomValidators.StringNotNullorEmpty(firstname, "firstname string is null or empty");
        //    CustomValidators.StringNotNullorEmpty(lastname, "lastname string is null or empty");*/
        //    CustomValidators.StringNotNullorEmpty(username, "username string is null or empty");
        //    CustomValidators.StringNotNullorEmpty(password, "password string is null or empty");
        //    CustomValidators.StringNotNullorEmpty(to, "'to' string is null or empty");

        //    var email_template = _emailTemplateRepository.get_template_by_type(template_name);

        //    if (email_template == null)
        //        return null;


        //    var body = email_template.Body;

        //    //body = body.Replace("{firstname}", firstname);
        //    //body = body.Replace("{lastname}", lastname);
        //    body = body.Replace("{username}", username);
        //    body = body.Replace("{password}", password);

        //    return new Email(email_template.Sender, to, email_template.Subject, body);
        //}
    }



}
