using System;
using System.Threading.Tasks;
using infrastructure.email.interfaces;
using System.Net;
using MimeKit;
using MailKit.Net.Smtp;

namespace infrastructure.email.services
{
    public class EmailMessageSender : IEmailSender
    {
        private readonly IEmailSettingsService _emailSettingsService;

        public EmailMessageSender(IEmailSettingsService emailSettingsService)
        {
            _emailSettingsService = emailSettingsService;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            Execute(email, subject, message).Wait();
            return Task.FromResult(0);
        }

        public async Task Execute(string emailTo, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_emailSettingsService.GetFromname(), _emailSettingsService.GetFrom()));
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
                client.Authenticate(_emailSettingsService.GetUsername(), _emailSettingsService.GetPassword());

                await client.SendAsync(message);
                client.Disconnect(true);
            }
        }
    }
}
