using Dapper;
using System.Linq;
using persistance.dapper.common;
using query.dto.user;

namespace persistance.dapper.repository
{
    public interface IUserQueries
    {
        UserIdentityDTO Get(string email, string password);
    }

    public class UserQueries : DapperService<UserIdentityDTO>, IUserQueries
    {
        private const string PROVIDER_LOCAL = "Local";

        public UserQueries(IDbConn connection)
            : base(connection)
        { } 
        
         public UserIdentityDTO Get(string email, string password)
         {
            var sql = @"SELECT 
                         u.id
                        ,ul.email
                        ,u.username
                        ,u.roles
                        ,u.avatar
                        ,ul.provider
                        FROM users_login ul
                        JOIN users u ON u.id = ul.user_id
                        WHERE ul.email = @myemail 
                        AND ul.password = @mypassword 
                        AND ul.provider = @myprovider
                        AND u.is_locked = FALSE
                        ;";

            using (var connection = SQLProfiler.GetOpenConnection(_connectionString))
             {              
                 return connection.Query<UserIdentityDTO>(sql, 
                     new {
                         myemail = email,
                         mypassword = password,
                         myprovider = PROVIDER_LOCAL
                     }
                 ).FirstOrDefault();                 
             }
         }
    }
}
