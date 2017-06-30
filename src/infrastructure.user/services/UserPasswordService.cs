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
                    login.UserPasswordReset.token,
                    login.UserPasswordReset.token_expiry_date
                    );
            var now = DateTime.Now;

            login.UserPasswordReset = new UserPassword {
                date_created = now,
                token = CryptographicService.GenerateRandomString(tokenLength),
                token_expiry_date = now.AddDays(expiryDays)
            };

            _userRepository.UpdateLogin(login);

            return new UserReset(
                login.User.id, 
                login.email, 
                login.UserPasswordReset.token, 
                login.UserPasswordReset.token_expiry_date
            );
        }
    }
}
