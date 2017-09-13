using Microsoft.Extensions.Options;

using infrastructure.user.interfaces;
using infrastucture.libs.strings;
using peruncore.Config;

namespace peruncore.Infrastructure.Auth
{
    public class AuthSchemeNameService : IAuthSchemeNameService
    {
        private readonly AuthSchemeSettings _authSchemeSettings;
        public AuthSchemeNameService(IOptions<AuthSchemeSettings> authSchemeSettings)
        {
            _authSchemeSettings = authSchemeSettings.Value;
        }

        public string getProviderName(string providerParameter)
        {
            providerParameter = providerParameter.FirstLetterToUpperCase();

            if (providerParameter == _authSchemeSettings.Google)
                return _authSchemeSettings.Google;

            if (providerParameter == _authSchemeSettings.Facebook)
                return _authSchemeSettings.Facebook;

            if (providerParameter == _authSchemeSettings.Twitter)
                return _authSchemeSettings.Twitter;

            throw new System.ArgumentException("Unknown provider.");
        }
        public string getDefaultProvider()
        {
            return _authSchemeSettings.Application;
        }
    }
}
