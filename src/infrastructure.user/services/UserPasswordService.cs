using infrastructure.user.entities;
using infrastructure.user.interfaces;
using infrastructure.user.models;
using infrastucture.libs.cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace infrastructure.user.services
{

    public class UserPasswordService: IUserPasswordService
    {
        // TODO: global settings
        private const int _saltLength = 16;
        private readonly IUserRepository _userRepository;
        private readonly ILogger _logger;
        private readonly IAuthSchemeNameService _authSchemeNameService;
        public UserPasswordService(IUserRepository userRepository, ILogger<UserPasswordService> logger, IAuthSchemeNameService authSchemeNameService)
        {
            _userRepository = userRepository;
            _logger = logger;
            _authSchemeNameService = authSchemeNameService;
        }

        public UserReset generateResetToken(string email, int tokenLength, int expiryDays)
        {
            var login = _userRepository.getByEmailWithResetInfo(email, _authSchemeNameService.getDefaultProvider());

            if (login == null)
            {
                _logger.LogError("User intents to generate Password Reset Token with invalid email." , new object[] { email });
                return null;
            }
            var now = DateTime.Now;

            // check if reset token exists 
            if (login.UserPasswordResets == null)
            {
                login.UserPasswordResets = new List<UserPasswordDb>();
            }

            var passwordReset = login.UserPasswordResets.Where(x => x.token_expiry_date >= now).SingleOrDefault();

            if (passwordReset != null)
            {
                return new UserReset(
                    login.user_id,
                    login.email,
                    passwordReset.token,
                    passwordReset.token_expiry_date
                    );
            }

            // create new password reset token 
            passwordReset = new UserPasswordDb
            {
                date_created = now,
                token = CryptographicService.GenerateRandomString(tokenLength),
                token_expiry_date = now.AddDays(expiryDays),
                date_modified = now
            };
            
            // update database
            login.UserPasswordResets.Add(passwordReset);
            _userRepository.updateLogin(login);

            // TODO: Automapper
            return new UserReset(
                login.User.id, 
                login.email,
                passwordReset.token,
                passwordReset.token_expiry_date
            );
        }

        public UserReset verifyToken(int userId, string token)
        {
            var login = _userRepository.getByIdWithResetInfo(userId, _authSchemeNameService.getDefaultProvider());
            var passwordReset = login.UserPasswordResets.Where(x => 
                                        x.token == token && 
                                        x.token_expiry_date >= DateTime.Now
                                        ).SingleOrDefault();
            if (passwordReset != null)
            {
                // TODO: Automapper
                return new UserReset(
                    login.User.id,
                    login.email,
                    passwordReset.token,
                    passwordReset.token_expiry_date
                );
            }

            return null;
        }

        public UserReset changePassword(int userid, string token, string password)
        {
            // TODO: duplicated code
            var login = _userRepository.getByIdWithResetInfo(userid, _authSchemeNameService.getDefaultProvider());
            var passwordReset = login.UserPasswordResets.Where(x =>
                                                 x.token == token &&
                                                 x.token_expiry_date >= DateTime.Now
                                                 ).SingleOrDefault();
            if (passwordReset == null)
            {
                return null;
            }

            var now = DateTime.Now;
            // TODO: duplicated code
            login.salt = CryptographicService.GenerateRandomString(_saltLength);
            login.passwd = CryptographicService.GenerateSaltedHash(password, login.salt);
            
            // TODO: unnecessary db call 
            login.UserPasswordResets.Where(x => x.id == passwordReset.id).Single().token_expiry_date = now;
            login.UserPasswordResets.Where(x => x.id == passwordReset.id).Single().date_modified = now;
            _userRepository.updateLogin(login);

            return new UserReset(
                login.User.id,
                login.email,
                passwordReset.token,
                passwordReset.token_expiry_date
            );
        }
    }
}
