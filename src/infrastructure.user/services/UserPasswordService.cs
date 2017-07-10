using infrastructure.user.entities;
using infrastructure.user.interfaces;
using infrastructure.user.models;
using infrastucture.libs.cryptography;
using System;
using System.Collections.Generic;
using System.Linq;

namespace infrastructure.user.services
{

    public class UserPasswordService: IUserPasswordService
    {
        private readonly IUserRepository _userRepository;
        public UserPasswordService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UserReset generateResetToken(string email, int tokenLength, int expiryDays)
        {
            var login = _userRepository.GetByEmailWithResetInfo(email);

            if (login == null)
            {
                // TODO: Throw an exception
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
                token_expiry_date = now.AddDays(expiryDays)
            };
            
            // update database
            login.UserPasswordResets.Add(passwordReset);
            _userRepository.UpdateLogin(login);

            return new UserReset(
                login.User.id, 
                login.email,
                passwordReset.token,
                passwordReset.token_expiry_date
            );
        }
    }
}
