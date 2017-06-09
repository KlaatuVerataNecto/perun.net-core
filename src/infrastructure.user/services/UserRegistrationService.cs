
using infrastructure.user.interfaces;
using System;

namespace infrastructure.user.services
{
    public interface IUserRegistrationService
    {
    }

    public class UserRegistrationService : IUserRegistrationService
    {
        private readonly IUserRepository _userRepository;
        public UserRegistrationService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
    }
}
