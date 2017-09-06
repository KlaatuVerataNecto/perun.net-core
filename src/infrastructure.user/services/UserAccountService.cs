
using infrastructure.user.entities;
using infrastructure.user.interfaces;
using infrastructure.user.models;
using infrastucture.libs.cryptography;
using Microsoft.Extensions.Logging;
using System;

namespace infrastructure.user.services
{
    public class UserAccountService : IUserAccountService
    {       
        private readonly IUserRepository _userRepository;
        private readonly ILogger _logger;

        public UserAccountService(IUserRepository userRepository, ILogger<UserAccountService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public UserIdentity ChangeUsername(int userid , string username, string token)
        {
            if (!_userRepository.isUsernameAvailable(username)) return null;
            var login = _userRepository.getByIdWithUserNameToken(userid);

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

            // TODO: Use Automapper
            return new UserIdentity(
                login.id,
                login.User.username,
                login.email,
                login.provider,
                login.User.roles,
                login.User.avatar);
        }
    }
}
