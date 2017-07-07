﻿
using infrastructure.user.entities;
using infrastructure.user.interfaces;
using infrastructure.user.models;
using infrastucture.libs.cryptography;
using System;

namespace infrastructure.user.services
{
    public class UserRegistrationService : IUserRegistrationService
    {
        private const int _saltLength = 16;
        private readonly IUserRepository _userRepository;

        public UserRegistrationService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UserIdentity Signup(string username, string email, string password, string provider, int saltLength)
        {
            if (!_userRepository.IsUsernameAvailable(username)) return null;
            if (!_userRepository.IsEmailAvailable(email)) return null;

            string salt = CryptographicService.GenerateRandomString(_saltLength);
            string hashed_password = CryptographicService.GenerateSaltedHash(password, salt);

            // TODO: Automapper
            var rightNow = DateTime.Now;
            var obj = new Login
            {
                email = email,
                passwd = hashed_password,
                salt = salt,
                provider = provider,
                date_created = rightNow,
                User = new User
                {
                    username = username,
                    is_locked = false,
                    date_created = rightNow,
                    last_seen = rightNow
                }
            };

            var login = _userRepository.AddLogin(obj);
            return new UserIdentity(login.id, login.User.username, login.email, login.provider, login.User.roles, login.User.avatar);
        }
    }
}
