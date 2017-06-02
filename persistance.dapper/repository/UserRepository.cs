using Dapper;
using System.Linq;
using persistance.dapper.common;
using query.dto.user;
using infrastructure.user.interfaces;
using infrastructure.user.models;

namespace persistance.dapper.repository
{
    public class UserRepository : DapperService<UserIdentityDTO>, IUserRepository
    {
        private const string PROVIDER_LOCAL = "Local";

        public UserRepository(IDbConn connection)
            : base(connection)
        { } 
        
         public UserAuthDB GetByEmail(string email)
         {
            var sql = @"SELECT 
                         u.id
                        ,ul.email
                        ,u.username
                        ,u.roles
                        ,u.avatar
                        ,ul.salt
                        ,ul.provider
                        FROM users_login ul
                        JOIN users u ON u.id = ul.user_id
                        WHERE ul.email = @myemail
                        AND ul.provider = @myprovider
                        AND u.is_locked = FALSE
                        ;";

            using (var connection = SQLProfiler.GetOpenConnection(_connectionString))
             {              
                 return connection.Query<UserAuthDB>(sql, 
                     new {
                         myemail = email,
                         myprovider = PROVIDER_LOCAL
                     }
                 ).FirstOrDefault();                 
             }
         }
    }
}
