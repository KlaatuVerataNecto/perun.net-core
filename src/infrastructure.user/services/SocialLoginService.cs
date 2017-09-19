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
        private readonly IAuthSchemeNameService _authSchemeNameService;
        private readonly ILogger _logger;

        public SocialLoginService(IUserRepository userRepository, IAuthSchemeNameService authSchemeNameService, ILogger<SocialLoginService> logger) 
        {
            _userRepository = userRepository;
            _authSchemeNameService = authSchemeNameService;
            _logger = logger;
        }

        public UserIdentity loginOrSignup(string nameIdentifier, string email,string firstname, string lastname, string provider, int currentLoginId)
        {
            var login = _userRepository.getByEmailAndProvider(email, provider);
            var rightNow = DateTime.Now;

            // NOT IN SESSION - login exists get authentication data
            if (login != null)
            {
                login.User.last_seen = rightNow;
                _userRepository.updateLogin(login);

                // TODO: Duplicated code 
                return new UserIdentity(
                    login.User.id,
                    login.id,
                    login.User.username,
                    login.email,
                    login.provider,
                    login.User.roles,
                    login.User.avatar);
            }

            // TODO: allow multiple logins with different providers, but no the same one
            // IN SESSION - login doesn't exist but let's check if user is currently logged with different login
            login = _userRepository.getById(currentLoginId);

            if (login != null)
            {
                login.User.last_seen = rightNow;

                var newLogin = new LoginDb
                {
                    email = email,
                    provider = provider,
                    date_created = rightNow,
                };

                login.User.Logins.Add(newLogin);
                _userRepository.updateLogin(login);

                // TODO: Duplicated code 
                return new UserIdentity(
                    login.User.id,
                    login.id,
                    login.User.username,
                    login.email,
                    login.provider,
                    login.User.roles,
                    login.User.avatar);
            }


            // NOT IN SESSION - check if user has Local account, if so add social login
            login = _userRepository.getByEmailAndProvider(email, _authSchemeNameService.getDefaultProvider());

            if (login != null)
            {               
                var loginNew = new LoginDb()
                {
                    email = login.email,
                    provider = provider,
                    date_created = rightNow                     
                };

                login.User.Logins.Add(loginNew);
                _userRepository.updateLogin(login);

                // TODO: Duplicated code 
                return new UserIdentity(
                    login.User.id,
                    login.id,
                    login.User.username,
                    login.email,
                    login.provider,
                    login.User.roles,
                    login.User.avatar);
            }

            // NOT IN SESSION - user doesn't exist, sign up 
            var obj = new LoginDb
            {
                email = email,
                provider = provider,
                date_created = rightNow,
                //external_id = nameIdentifier,
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
            obj.User.username = StringService.ReplaceWhitespace(firstname+lastname+obj.User.id,"");

            // persist
            _userRepository.updateLogin(obj);

            return new UserIdentity(
                obj.User.id,
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
