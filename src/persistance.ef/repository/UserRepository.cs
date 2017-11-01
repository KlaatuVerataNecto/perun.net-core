using infrastructure.user.interfaces;
using persistance.ef.common;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using infrastructure.user.entities;
using System;
using System.Collections.Generic;

namespace persistance.ef.repository
{
    public class UserRepository : IUserRepository
    {
        private IEFContext _efContext;

        public UserRepository(IEFContext context)
        {
            _efContext = context;
        }

        public bool isUsernameAvailable(string username)
        {
            if (string.IsNullOrEmpty(username)) return false;
            return !_efContext.Users.Any(x => x.username.ToLower() == username.ToLower());
        }

        public bool isEmailAvailable(string email)
        {
            if (string.IsNullOrEmpty(email)) return false;
            return !_efContext.Logins.Any(x => x.email.ToLower() == email.ToLower());
        }

        public UserDb getUserById(int id)
        {
            var obj = _efContext.Users   
                                .Where(x => x.id == id
                                            && x.is_locked == false
                                           ).SingleOrDefault();

            return obj;
        }

        public LoginDb getByEmailAndProvider(string email, string provider)
        {
            var obj = _efContext.Logins
                                .Include(l => l.User)
                                .Where(x => x.email == email
                                                && x.provider == provider
                                                && x.User.is_locked == false
                                                ).SingleOrDefault();
            if (obj == null) return null;

            return obj;
        }

        public LoginDb getByEmail(string email)
        {
            var obj = _efContext.Logins
                                .Include(l => l.User)
                                .Where(x => x.email == email
                                                && x.User.is_locked == false
                                                ).FirstOrDefault(); // multiple logins with the same email are acceptable, we need only first 
            if (obj == null) return null;

            return obj;
        }

        public LoginDb getByEmailWithResetInfo(string email, string provider)
        {
            var obj = _efContext.Logins
                                .Include(l => l.User)
                                .Include(l => l.UserPasswordResets)
                                .Include(l => l.UserEmailChanges)
                                .Where(x => x.email == email
                                                && x.provider == provider
                                                && x.User.is_locked == false
                                                ).SingleOrDefault();
            if (obj == null) return null;

            return obj;
        }

        public LoginDb getByIdWithResetInfo(int id, string provider)
        {
            var obj = _efContext.Logins
                                .Include(l => l.User)
                                .Include(l => l.UserPasswordResets)
                                .Include(l => l.UserEmailChanges)
                                .Where(x => x.user_id == id
                                                && x.provider == provider
                                                && x.User.is_locked == false
                                                ).SingleOrDefault();

            return obj;
        }

        public LoginDb getByIdWithUserNameToken(int id)
        {
            var obj = _efContext.Logins
                                .Include(u=>u.User)
                                .Include(l =>l.User.UsernameToken)
                                .Where(x => x.User.id == id
                                            && x.User.is_locked == false
                                           ).SingleOrDefault();

            return obj;
        }

        public List<LoginDb> getLoginsByUserId(int id)
        {
            return _efContext.Logins
                               .Include(u => u.User)
                               .Where(x => x.User.id == id
                                           && x.User.is_locked == false
                                          ).ToList();
        }

        public LoginDb addLogin(LoginDb obj)
        {
            _efContext.Logins.Add(obj);
            _efContext.SaveChanges();

            return obj;
        }

        public LoginDb getById(int id)
        {
            return _efContext.Logins
                               .Include(u => u.User)
                               .Where(x => x.id == id
                                           && x.User.is_locked == false
                                          ).SingleOrDefault();
        }

        public LoginDb getIdAndProvider(int id, string provider)
        {
            var obj = _efContext.Logins
                                .Include(l => l.User)
                                .Where(x => x.User.id == id
                                                && x.provider == provider
                                                && x.User.is_locked == false
                                                ).SingleOrDefault();
            if (obj == null) return null;

            return obj;
        }

        public void updateLogin(LoginDb obj)
        {
            _efContext.Logins.Update(obj);
            _efContext.SaveChanges();
        }

        public void updateUser(UserDb obj)
        {
            _efContext.Users.Update(obj);
            _efContext.SaveChanges();
        }
    }
}
