using infrastructure.user.interfaces;
using infrastructure.user.models;
using persistance.ef.common;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using persistance.ef.entity;
using System;

namespace persistance.ef.repository
{
    public class UserRepository : IUserRepository
    {
        private IEFContext _efContext;
        private const string PROVIDER_LOCAL = "Local";

        public UserRepository(IEFContext context)
        {
            _efContext = context;
        }

        public bool IsUsernameAvailable(string username)
        {
            return !_efContext.Users.Any(x => x.username.ToLower() == username.ToLower());         
        }

        public bool IsEmailAvailable(string email)
        {
            return !_efContext.Logins.Any(x => x.email.ToLower() == email.ToLower());
        }

        public UserAuthDB GetByEmail(string email)
         {
            var obj = _efContext.Logins
                                .Include(l => l.User)
                                .Where(x => x.email == email 
                                                && x.provider == PROVIDER_LOCAL 
                                                && x.User.is_locked == false
                                                ).SingleOrDefault();
            if (obj == null) return null; 
            
            // TODO: Automapper
            return new UserAuthDB
            {
                id = obj.User.id,
                username = obj.User.username,
                salt = obj.salt,
                password = obj.passwd,
                email = obj.email,
                provider = obj.provider,
                roles = obj.User.roles,
                avatar = obj.User.avatar,
            };
         }

        public UserAuthDB Add(string username, string email, string hashed_password, string salt, string provider)
        {
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

            _efContext.Logins.Add(obj);
            _efContext.SaveChanges();

             // TODO: Automapper
            return new UserAuthDB
            {
                id = obj.User.id,
                username = obj.User.username,
                salt = obj.salt,
                password = obj.passwd,
                email = obj.email,
                provider = obj.provider,
                roles = obj.User.roles,
                avatar = obj.User.avatar,
            };
        }
    }
}
