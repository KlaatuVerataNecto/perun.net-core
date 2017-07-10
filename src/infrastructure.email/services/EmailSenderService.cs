using System;
using System.Threading.Tasks;
using infrastructure.email.interfaces;
using System.Net;
using MimeKit;
using MailKit.Net.Smtp;

namespace infrastructure.email.services
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly IEmailSettingsService _emailSettingsService;

        public EmailSenderService(IEmailSettingsService emailSettingsService)
        {
            _emailSettingsService = emailSettingsService;
        }

        public Task SendEmailAsync(string emailTo, string emailFrom, string emailFromName,  string subject, string body)
        {
            Execute(emailTo, emailFrom,emailFromName, subject, body).Wait();
            return Task.FromResult(0);
        }

        public async Task Execute(string emailTo, string emailFrom, string emailFromName, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(emailFromName, emailFrom));
            message.To.Add(new MailboxAddress(emailTo));
            message.Subject = subject;

            message.Body = new TextPart("html")
            {
                Text = body
            };

            using (var client = new SmtpClient())
            {
                // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Connect(_emailSettingsService.GetHost(), _emailSettingsService.GetPort(), false);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                // Note: only needed if the SMTP server requires authentication
                // if you use Gmail, allow access by less secure apps  
                client.Authenticate(_emailSettingsService.GetUsername(), _emailSettingsService.GetPassword());

                await client.SendAsync(message);
                client.Disconnect(true);
            }
        }
    }
}
