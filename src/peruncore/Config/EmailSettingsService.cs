using infrastructure.email.interfaces;
using Microsoft.Extensions.Options;

namespace peruncore.Config
{


    public class EmailSettingsService : IEmailSettingsService
    {
        private readonly EmailSettings _emailSettings;

        public EmailSettingsService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }
        public string GetHost() {
            return _emailSettings.Host;
        }
        public int GetPort()
        {
            return _emailSettings.Port;
        }
        public bool GetIsSSL()
        {
            return _emailSettings.isSSL;
        }
        public string GetFrom()
        {
            return _emailSettings.From;
        }
        public string GetFromname()
        {
            return _emailSettings.Fromname;
        }
        public string GetUsername()
        {
            return _emailSettings.Username;
        }
        public string GetPassword()
        {
            return _emailSettings.Password;
        }
    }
}
