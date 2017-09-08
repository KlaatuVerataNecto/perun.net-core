
using infrastructure.user.entities;
using infrastructure.user.interfaces;
using infrastructure.user.models;
using infrastucture.libs.cryptography;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace infrastructure.user.services
{
    public class UserAccountService : IUserAccountService
    {       
        private readonly IUserRepository _userRepository;
        private readonly IAuthSchemeSettingsService _authSchemeSettingsService;
        private readonly ILogger _logger;

        public UserAccountService(IUserRepository userRepository, IAuthSchemeSettingsService authSchemeSettingsService, ILogger<UserAccountService> logger)
        {
            _userRepository = userRepository;
            _authSchemeSettingsService = authSchemeSettingsService;
            _logger = logger;
        }

        public UserIdentity changeUsername(int userid , string username, string token)
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
                login.User.id,
                login.User.username,
                login.email,
                login.provider,
                login.User.roles,
                login.User.avatar);
        }

        public List<UserLogin> getLoginsByUserId(int userid)
        {
            var list = _userRepository.getLoginsByUserId(userid);
            var myLogins = new List<UserLogin>();

            foreach (var l in list)
            {
                myLogins.Add(new UserLogin(l.User.id, l.email, l.provider, _authSchemeSettingsService.GetDefaultProvider()));
            }

            return myLogins;
        }
    }
}
