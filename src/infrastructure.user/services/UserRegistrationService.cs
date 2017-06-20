
using infrastructure.user.interfaces;
using infrastructure.user.models;
using infrastucture.libs.cryptography;
using System;

namespace infrastructure.user.services
{
    public interface IUserRegistrationService
    {
        UserIdentity Signup(string username, string email, string password, string provider, int saltLength);
    }

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

            var userAuthDB = _userRepository.Add(username, email, hashed_password, salt, provider);
            return new UserIdentity(userAuthDB.id, userAuthDB.username, userAuthDB.email, userAuthDB.provider, userAuthDB.roles, userAuthDB.avatar);
        }
    }
}
