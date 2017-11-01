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

        public UserIdentity loginOrSignup(string nameIdentifier, string email, string firstname, string lastname , string provider, int currentLoginId)
        {
            LoginDb userLogin = null;
            LoginDb loginInSession = null;  
            var rightNow = DateTime.Now;

            // fetch input login by email
            userLogin = _userRepository.getByEmail(email);

            // get login of the user in session, currentLoginId is 0 when there is no session            
            loginInSession = (currentLoginId > 0)? _userRepository.getById(currentLoginId): null;
            
            // user not in session and input login doesn't exist
            if(loginInSession == null && userLogin == null)
            {
                // new login, signup up
                return this.signup(nameIdentifier, email, firstname, lastname,  provider, rightNow);
            }

            // user not in session and input login exists
            if (loginInSession == null && userLogin != null)
            {
                if(userLogin.provider == provider)
                {
                    // provider matches, login in
                    return this.login(userLogin, rightNow);
                } 
                else
                {
                    // provider doesn't match email already taken
                    _logger.LogError("User in session adds social login with email that already exists."
                        , new {
                                userLogin_id = userLogin.id,
                                userLogin_email = userLogin.email,
                                userLogin_provider = provider,
                                inputEmail = email,
                                inputProvider = provider
                        });

                    return null;
                }
            }

            //user in session and input login doesn't exist
            if (loginInSession != null && userLogin == null)
            {
                // user in session authenticated with application login or social login 
                // and it's adding new login
                return this.addNewLogin(loginInSession,nameIdentifier, email, firstname, lastname, provider, rightNow);
            }

            // user in session and input login does exist
            if (loginInSession != null && userLogin != null)
            {
                // user in session authenticated with application login or social login 
                // and trying to add new one that is already used
                // throw error 
                _logger.LogError("User in session adds social login with email that already exists."
                    , new
                    {
                        userLogin_id = userLogin.id,
                        userLogin_email = userLogin.email,
                        userLogin_provider = provider,
                        sessionLogin_id = loginInSession.id,
                        sessionLogin_email = loginInSession.email,
                        sessionLogin_provider = loginInSession.provider,
                        inputEmail = email,                       
                        inputProvider = provider
                    });
            }

            return null;           

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

        private UserIdentity login(LoginDb login, DateTime rightNow)
        {
            login.User.last_seen = rightNow;
            _userRepository.updateLogin(login);

            // TODO: Automapper profile for LoginDB > UserIdentity
            return new UserIdentity(
                login.User.id,
                login.id,
                login.User.username,
                login.email,
                login.provider,
                login.User.roles,
                login.User.avatar);
        }

        private UserIdentity addNewLogin(LoginDb loginInSession, string nameIdentifier, string email, string firstname, string lastname, string provider, DateTime rightNow)
        {
            loginInSession.User.last_seen = rightNow;

            var newLogin = new LoginDb
            {
                email = email,
                provider = provider,
                date_created = rightNow,
            };

            loginInSession.User.Logins.Add(newLogin);
            _userRepository.updateLogin(loginInSession);

            // TODO: Automapper profile for LoginDB > UserIdentity
            return new UserIdentity(
                    loginInSession.User.id,
                    loginInSession.id,
                    loginInSession.User.username,
                    loginInSession.email,
                    loginInSession.provider,
                    loginInSession.User.roles,
                    loginInSession.User.avatar);
        }

        private UserIdentity signup(string nameIdentifier, string email, string firstname, string lastname, string provider, DateTime rightNow)
        {
            // construct DB entity
            var obj = new LoginDb
            {
                email = email,
                provider = provider,
                date_created = rightNow,
                //external_id = nameIdentifier, // TODO: save external id
                User = new UserDb
                {
                    is_locked = false,
                    date_created = rightNow,
                    last_seen = rightNow,
                    UsernameToken = new UserUsernameDb
                    {
                        token = CryptographicService.GenerateRandomString(20), // TODO: move to config
                        date_created = rightNow,
                    }
                }
            };

            // persist changes 
            _userRepository.addLogin(obj);

            // generate "unique" username (TODO: temporary solution)
            obj.User.username = StringService.ReplaceWhitespace(firstname + lastname + obj.User.id, "");

            // persist
            _userRepository.updateLogin(obj);

            // TODO: Automapper profile for LoginDB > UserIdentity
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
    }    
}
