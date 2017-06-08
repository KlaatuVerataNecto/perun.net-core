using infrastructure.user.interfaces;
using infrastructure.user.models;
using persistance.ef.common;
using System.Linq;

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
        
         public UserAuthDB GetByEmail(string email)
         {
            var obj = _efContext.Logins.Where(x => x.email == email && 
                                                x.provider == PROVIDER_LOCAL &&
                                                x.User.is_locked == false)
                                                .SingleOrDefault();

            return new UserAuthDB
            {
                id = obj.User.id,
                username = obj.User.username,
                email = obj.email,
                provider = obj.provider,
                roles = obj.User.roles,
                avatar = obj.User.avatar
            };
         }
    }
}
