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

        public UserQueries(IDbConn connection)
            : base(connection)
        { } 
        
         public UserIdentityDTO Get(string email, string password)
         {
            var sql = @"SELECT * FROM users WHERE email = @myemail AND password = @mypassword;";

            using (var connection = SQLProfiler.GetOpenConnection(_connectionString))
             {              
                 return connection.Query<UserIdentityDTO>(sql, 
                     new {
                         myemail = email,
                         mypassword = password
                     }
                 ).FirstOrDefault();                 
             }
         }
    }
}
