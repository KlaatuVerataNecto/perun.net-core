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
            UserAuthDB userAuthDB = _userRepository.GetByEmail(email);

            if (userAuthDB == null) return null;
            string hashed_password = CryptographicService.GenerateSaltedHash(password, userAuthDB.salt);
            
            if (userAuthDB.password != hashed_password) return null;
            return new UserIdentity(userAuthDB.id, userAuthDB.username, userAuthDB.email, userAuthDB.provider, userAuthDB.roles, userAuthDB.avatar);
        }
    }    
}
