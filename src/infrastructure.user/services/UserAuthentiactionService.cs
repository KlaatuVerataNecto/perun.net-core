using infrastructure.user.interfaces;
using infrastructure.user.models;
using infrastucture.libs.cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.user.services
{
    public interface IUserAuthentiactionService
    {
        UserIdentity login(string email, string password);
    }

    public class UserAuthentiactionService : IUserAuthentiactionService
    {
        private readonly IUserRepository _userRepository;

        public UserAuthentiactionService(IUserRepository userRepository) 
        {
            _userRepository = userRepository;
        }

        public UserIdentity login(string email, string password)
        {
            var login = _userRepository.GetByEmail(email);

            if (login == null) return null;
            string hashed_password = CryptographicService.GenerateSaltedHash(password, login.salt);
            
            if (login.passwd != hashed_password) return null;
            return new UserIdentity(login.id, login.User.username, login.email, login.provider, login.User.roles, login.User.avatar);
        }
    }    
}
