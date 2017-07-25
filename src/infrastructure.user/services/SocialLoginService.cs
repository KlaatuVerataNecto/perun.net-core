using infrastructure.user.entities;
using infrastructure.user.interfaces;
using infrastructure.user.models;
using infrastucture.libs.cryptography;
using infrastucture.libs.strings;
using Microsoft.Extensions.Logging;
using System;

namespace infrastructure.user.services
{
    public class SocialLoginService : ISocialLoginService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthSchemeSettingsService _authSchemeSettingsService;
        private readonly ILogger _logger;

        public SocialLoginService(IUserRepository userRepository, IAuthSchemeSettingsService authSchemeSettingsService, ILogger<SocialLoginService> logger) 
        {
            _userRepository = userRepository;
            _authSchemeSettingsService = authSchemeSettingsService;
            _logger = logger;
        }

        public UserIdentity loginOrSignup(string nameIdentifier, string email,string firstname, string lastname, string provider)
        {
            var loggedUser = _userRepository.getByEmailAndProvider(email, provider);
            var rightNow = DateTime.Now;

            // login 
            if (loggedUser != null)
            {
                loggedUser.User.last_seen = rightNow;
                _userRepository.updateLogin(loggedUser);
                return new UserIdentity(
                    loggedUser.id,
                    loggedUser.User.username,
                    loggedUser.email,
                    loggedUser.provider,
                    loggedUser.User.roles,
                    loggedUser.User.avatar);
            }

            // sorry email taken
            if (!_userRepository.isEmailAvailable(email)) return null;

            // register 
            var obj = new LoginDb
            {
                email = email,
                provider = provider,
                date_created = rightNow,
                User = new UserDb
                {
                    is_locked = false,
                    date_created = rightNow,
                    last_seen = rightNow,
                    UsernameToken = new UserUsernameDb
                    {
                        token =  CryptographicService.GenerateRandomString(20),
                        date_created = rightNow,           
                    }
                }
            };

            // persist
            _userRepository.addLogin(obj);

            // generate "unique" username (TODO: temporary solution)
            obj.User.username = StringService.ReplaceWhitespace(
                firstname+lastname+obj.User.id
             ,"");

            // persist
            _userRepository.updateLogin(obj);

            return new UserIdentity(
                obj.id,
                obj.User.username,
                obj.email,
                obj.provider,
                obj.User.roles,
                obj.User.avatar,
                true // set flag in order to ask user to pick username
                );
        }
        public UsernameToken getTokenByUserId(int userid)
        {
            var user = _userRepository.getByIdWithUserNameToken(userid);
            if(user == null || user.UsernameToken == null)
            {
                _logger.LogError("Some setting username without valid string.", new object[] { userid });
                return null;
            }

            return new UsernameToken(user.id, user.UsernameToken.token);
        }
    }    
}
