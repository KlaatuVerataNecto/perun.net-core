using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;

using infrastructure.user.entities;
using infrastructure.user.interfaces;
using infrastructure.user.models;
using infrastucture.libs.cryptography;

namespace infrastructure.user.services
{
    public class UserAccountService : IUserAccountService
    {       
        private readonly IUserRepository _userRepository;
        private readonly IAuthSchemeNameService _authSchemeNameService;
        private readonly IUserAuthentiactionService _userAuthentiactionService;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public UserAccountService(
            IUserRepository userRepository,
            IUserAuthentiactionService userAuthentiactionService,
            IAuthSchemeNameService authSchemeNameService,
            IMapper mapper,
            ILogger<UserAccountService> logger)
        {
            _userRepository = userRepository;
            _userAuthentiactionService = userAuthentiactionService;
            _authSchemeNameService = authSchemeNameService;
            _mapper = mapper;
            _logger = logger;
        }

        public UserProfile getUserProfile(int userId)
        {
            var user = _userRepository.getUserById(userId);
            if (user == null)
            {
                return null;
            }
            return _mapper.Map<UserProfile>(user);
        }

        public UserIdentity changeUsernameByToken(int userId, string username, string token)
        {
            if (!_userRepository.isUsernameAvailable(username)) return null;
            var login = _userRepository.getByIdWithUserNameToken(userId);

            if(login == null || 
               login.User == null ||
               login.User.UsernameToken == null ||
               login.User.UsernameToken.token != token)
            {
                _logger.LogError("User tries to change username after social login without valid token.");
                return null;
            }

            login.User.username = username;
            login.User.UsernameToken.token = null;
            _userRepository.updateLogin(login);

            return _mapper.Map<UserIdentity>(login);
        }

        public string changeUsername(int userId, string username)
        {
            if (!_userRepository.isUsernameAvailable(username)) return null;
            var user = _userRepository.getUserById(userId);

            if (user == null)
            {
                _logger.LogError("User tries to change username after social login without valid token.");
                return null;
            }

            user.username = username;
            _userRepository.updateUser(user);

            return username;
        }

        public string changeAvatar(int userId, string avatarImage)
        {
            var user = _userRepository.getUserById(userId);
            if (user == null)
            {
                _logger.LogError("Invalid user tries to change avatar.");
                return null;
            }
            

            user.avatar = avatarImage;
            _userRepository.updateUser(user);

            return avatarImage;
        }

        public string changeCover(int userId, string coverImage)
        {
            var user = _userRepository.getUserById(userId);
            if (user == null)
            {
                _logger.LogError("Invalid user tries to change avatar.");
                return null;
            }


            user.cover = coverImage;
            _userRepository.updateUser(user);

            return coverImage;
        }

        // TODO: Redundand data retrieved
        public UserUsername getUsernameByUserId(int userId)
        {
            var user = _userRepository.getUserById(userId);

            if (user == null)
            {
                _logger.LogError("Application login not found for user: .", new object[] { userId });
                return null;
            }

            return new UserUsername(
                user.id,
                user.username
            );
        }


        public UserLogin getApplicationLoginById(int userId)
        { 
            var login = _userRepository.getIdAndProvider(userId, _authSchemeNameService.getDefaultProvider());

            if (login == null)
            {
                _logger.LogError("Application login not found for user: .", new object[] { userId });
                return null;
            }

            return _mapper.Map<UserLogin>(login);        
        }

        // TODO: validate password too
        public EmailChange createEmailChangeRequest(int userId, string password, string newemail, int tokenLength, int expiryDays)
        {
            if (!_userRepository.isEmailAvailable(newemail)) return null;
            var login = _userRepository.getByIdWithResetInfo(userId, _authSchemeNameService.getDefaultProvider());
            if (login == null)
            {
                _logger.LogError("User intents to generate Email Change Token with invalid userid.", new object[] { userId });
                return null;
            }

           if(_userAuthentiactionService.login(login.email, password) == null)
            {
                _logger.LogError("User intents to generate Email Change Token with invalid password.", new object[] { login.email, });
                return null;
            }

            var now = DateTime.Now;

            // check if email change token exists 
            if (login.UserEmailChanges == null)
            {
                login.UserEmailChanges = new List<UserEmailDb>();
            } 

            var emailChange = login.UserEmailChanges.Where(x => x.token_expiry_date >= now).SingleOrDefault();

            if (emailChange != null)
            {
                return new EmailChange(
                    login.user_id,
                    login.email,
                    newemail,
                    emailChange.token,
                    emailChange.token_expiry_date
                    );
            }

            // create new email change token 
            emailChange = new UserEmailDb
            {
                date_created = now,
                token = CryptographicService.GenerateRandomString(tokenLength),
                token_expiry_date = now.AddDays(expiryDays),
                date_modified = now,
                newemail = newemail
            };

            // update database
            login.UserEmailChanges.Add(emailChange);
            _userRepository.updateLogin(login);

            return new EmailChange(
                login.User.id,
                login.email,
                emailChange.newemail,
                emailChange.token,
                emailChange.token_expiry_date
            );
        }

        public EmailChanged applyEmailByToken(int userId, string token)
        {
            var login = _userRepository.getByIdWithResetInfo(userId, _authSchemeNameService.getDefaultProvider());

            if (login == null)
            {
                return null;
            }

            var emailChange = login.UserEmailChanges.Where(x =>
                                        x.token == token &&
                                        x.token_expiry_date >= DateTime.Now
                                        ).SingleOrDefault();


            if (emailChange != null)
            {
                var emailChanged = new EmailChanged(login.User.id, emailChange.newemail, login.email);

                login.email = emailChange.newemail;
                _userRepository.updateLogin(login);
                cancelEmailActivation(userId);
                return emailChanged;
            }

            return null;
        }

        public List<UserLogin> getLoginsByUserId(int userId)
        {
            var list = _userRepository.getLoginsByUserId(userId);
            var myLogins = new List<UserLogin>();

            foreach (var l in list)
            {
                myLogins.Add(_mapper.Map<UserLogin>(l));
            }

            return myLogins;
        }

        public string getPendingNewEmailActivation(int userId)
        {
            var login = _userRepository.getByIdWithResetInfo(userId, _authSchemeNameService.getDefaultProvider());
            return login.UserEmailChanges.Where(x =>x.token_expiry_date >= DateTime.Now).Select(x=>x.newemail).SingleOrDefault();
        }

        public void cancelEmailActivation(int userId)
        {
            var login = _userRepository.getByIdWithResetInfo(userId, _authSchemeNameService.getDefaultProvider());
            login.UserEmailChanges.ToList().ForEach(c => { c.token_expiry_date = DateTime.Now.AddDays(-1); c.date_modified = DateTime.Now; });
            _userRepository.updateLogin(login);
        }

        public bool changePassword(int userId, string currentPassowrd, string newPassowrd, int saltLength)
        {
            var login = _userRepository.getIdAndProvider(userId, _authSchemeNameService.getDefaultProvider());

            if (login == null)
                return false;

            string hashed_password = CryptographicService.GenerateSaltedHash(currentPassowrd, login.salt);

            if (login.passwd != hashed_password)
                return false;

            login.salt = CryptographicService.GenerateRandomString(saltLength);
            login.passwd = CryptographicService.GenerateSaltedHash(newPassowrd, login.salt);
            _userRepository.updateLogin(login);

            return true;
        }
    }
}
