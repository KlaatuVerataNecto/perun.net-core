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
        private const string PROVIDER_LOCAL = "Local";

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

        public LoginDb getByEmail(string email)
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

        public LoginDb getByEmailWithResetInfo(string email)
        {
            var obj = _efContext.Logins
                                .Include(l => l.User)
                                .Include(l => l.UserPasswordResets)
                                .Include(l => l.UserEmailChanges)
                                .Where(x => x.email == email
                                                && x.provider == PROVIDER_LOCAL
                                                && x.User.is_locked == false
                                                ).SingleOrDefault();
            if (obj == null) return null;

            return obj;
        }

        public LoginDb getByIdWithResetInfo(int id)
        {
            var obj = _efContext.Logins
                                .Include(l => l.User)
                                .Include(l => l.UserPasswordResets)
                                .Include(l => l.UserEmailChanges)
                                .Where(x => x.user_id == id
                                                && x.provider == PROVIDER_LOCAL
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
