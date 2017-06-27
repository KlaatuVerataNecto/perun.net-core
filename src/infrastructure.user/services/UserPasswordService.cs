using infrastructure.user.entities;
using infrastructure.user.interfaces;
using infrastructure.user.models;
using infrastucture.libs.cryptography;
using System;
using System.Collections.Generic;
using System.Text;

namespace infrastructure.user.services
{
    public interface IUserPasswordService
    {
        UserReset generateResetToken(string email, int tokenLength, int expiryDays);
    }
    public class UserPasswordService: IUserPasswordService
    {
        private readonly IUserRepository _userRepository;
        public UserPasswordService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UserReset generateResetToken(string email, int tokenLength, int expiryDays)
        {
            var login = _userRepository.GetByEmail(email);

            if (login == null)
                return null;

            if (login.UserPasswordReset != null)
                return new UserReset(
                    login.user_id, 
                    login.email,
                    login.UserPasswordReset.password_token,
                    login.UserPasswordReset.password_token_expiry_date
                    );
            var now = DateTime.Now;

            login.UserPasswordReset = new UserPassword {
                date_created = now,
                password_token = CryptographicService.GenerateRandomString(tokenLength),
                password_token_expiry_date = now.AddDays(expiryDays)
            };

            _userRepository.AddUserPassword();

            return new UserReset(
                login.User.id, 
                login.email, 
                login.UserPasswordReset.password_token, 
                login.UserPasswordReset.password_token_expiry_date
            );
        }
    }
}
