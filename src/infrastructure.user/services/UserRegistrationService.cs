
using AutoMapper;
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
        private readonly IMapper _mapper;

        public UserRegistrationService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public UserIdentity signup(string username, string email, string password, string provider, int saltLength)
        {
            if (!_userRepository.isUsernameAvailable(username)) return null;
            if (!_userRepository.isEmailAvailable(email)) return null;

            string salt = CryptographicService.GenerateRandomString(_saltLength);
            string hashed_password = CryptographicService.GenerateSaltedHash(password, salt);

            var rightNow = DateTime.Now;
            var obj = new LoginDb
            {
                email = email,
                passwd = hashed_password,
                salt = salt,
                provider = provider,
                date_created = rightNow,
                User = new UserDb
                {
                    username = username,
                    is_locked = false,
                    date_created = rightNow,
                    last_seen = rightNow
                }
            };

            var login = _userRepository.addLogin(obj);

            return _mapper.Map<UserIdentity>(login);
        }
    }
}
