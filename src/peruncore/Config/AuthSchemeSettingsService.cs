using infrastructure.user.interfaces;
using Microsoft.Extensions.Options;

namespace peruncore.Config
{
    public class AuthSchemeSettingsService: IAuthSchemeSettingsService
    {
        private readonly AuthSchemeSettings _settings;

        public AuthSchemeSettingsService(IOptions<AuthSchemeSettings> settings)
        {
            _settings = settings.Value;
        }
        public string GetDefaultProvider()
        {
            return _settings.Application;
        }

    }
}
