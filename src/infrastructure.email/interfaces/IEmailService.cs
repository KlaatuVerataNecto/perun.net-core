using infrastructure.email.models;
using System;

namespace infrastructure.email.interfaces
{
    public interface IEmailService
    {
        void sendPasswordReminder(string emailTo, string link, DateTime expiryDate);
        void sendEmailChangeActivation(string emailTo, string link, DateTime expiryDate);
        //bool sendUserAccountInfo(MailMessage mail, string username, string password /*string firstname, string lastname*/);        
    }
}
