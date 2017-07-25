using infrastucture.libs.strings;
using Microsoft.Extensions.Options;
using peruncore.Config;

namespace peruncore.Infrastructure.Auth
{
    public interface IAuthProviderValidationService
    {
        string GetProviderName(string providerParameter);
    }
    public class AuthProviderValidationService : IAuthProviderValidationService
    {
        private readonly AuthSchemeSettings _authSchemeSettings;
        public AuthProviderValidationService(IOptions<AuthSchemeSettings> authSchemeSettings)
        {
            _authSchemeSettings = authSchemeSettings.Value;
        }

        public string GetProviderName(string providerParameter)
        {
            providerParameter = providerParameter.FirstLetterToUpperCase();

            if (providerParameter == _authSchemeSettings.Google)
                return _authSchemeSettings.Google;

            if (providerParameter == _authSchemeSettings.Facebook)
                return _authSchemeSettings.Facebook;

            throw new System.ArgumentException("Unknown provider.");
        }
    }
}
