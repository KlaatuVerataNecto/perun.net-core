using infrastructure.email.models;
using System;

namespace infrastructure.email.interfaces
{
    public interface IEmailService
    {
        void sendPasswordReminder(string emailTo, string token, DateTime expiryDate);
        //    bool sendChangeEmail(MailMessage mail, string url);
        //    bool sendUserAccountInfo(MailMessage mail, string username, string password /*string firstname, string lastname*/);
        //}
    }
}
