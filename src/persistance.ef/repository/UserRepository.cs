using infrastructure.user.interfaces;
using persistance.ef.common;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using infrastructure.user.entities;

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

        public Login GetByEmail(string email)
        {
            var obj = _efContext.Logins
                                .Include(l => l.User)
                                .Where(x => x.email == email
                                                && x.provider == PROVIDER_LOCAL
                                                && x.User.is_locked == false
                                                ).SingleOrDefault();
            if (obj == null) return null;

            return obj;
        }

        public Login AddLogin(Login obj)
        {
            _efContext.Logins.Add(obj);
            _efContext.SaveChanges();

            return obj;
        }

        public void UpdateLogin(Login obj)
        {
            _efContext.Logins.Update(obj);
            _efContext.SaveChanges();
        }
    }
}
