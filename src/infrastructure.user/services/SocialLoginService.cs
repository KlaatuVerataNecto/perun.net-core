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

            // authenticate user
            if (loggedUser != null)
            {
                loggedUser.User.last_seen = rightNow;
                _userRepository.updateLogin(loggedUser);

                // TODO: Duplicated code 
                return new UserIdentity(
                    loggedUser.User.id,
                    loggedUser.User.username,
                    loggedUser.email,
                    loggedUser.provider,
                    loggedUser.User.roles,
                    loggedUser.User.avatar);
            }

            // check if user has Local account, if so add it., 
            loggedUser = _userRepository.getByEmailAndProvider(email, _authSchemeSettingsService.GetDefaultProvider());

            if (loggedUser != null)
            {               
                var login = new LoginDb()
                {
                    email = loggedUser.email,
                    provider = provider,
                    date_created = rightNow                     
                };

                loggedUser.User.Logins.Add(login);
                _userRepository.updateLogin(loggedUser);

                // TODO: Duplicated code 
                return new UserIdentity(
                    login.User.id,
                    login.User.username,
                    login.email,
                    login.provider,
                    login.User.roles,
                    login.User.avatar);
            }
            
            // user doesn't exist, register 
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
                obj.User.id,
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
            var login = _userRepository.getByIdWithUserNameToken(userid);

            if (login == null ||
               login.User == null ||
               login.User.UsernameToken == null)
            { 
                _logger.LogError("Some setting username without valid string.", new object[] { userid });
                return null;
            }
            return new UsernameToken(login.User.id, login.User.UsernameToken.token);
        }
    }    
}
