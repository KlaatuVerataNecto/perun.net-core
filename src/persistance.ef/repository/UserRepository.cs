using infrastructure.user.interfaces;
using persistance.ef.common;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using infrastructure.user.entities;

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
            return !_efContext.Users.Any(x => x.username.ToLower() == username.ToLower());
        }

        public bool isEmailAvailable(string email)
        {
            return !_efContext.Logins.Any(x => x.email.ToLower() == email.ToLower());
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


        public LoginDb addLogin(LoginDb obj)
        {
            _efContext.Logins.Add(obj);
            _efContext.SaveChanges();

            return obj;
        }

        public void updateLogin(LoginDb obj)
        {
            _efContext.Logins.Update(obj);
            _efContext.SaveChanges();
        }
    }
}
