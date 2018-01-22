using Dapper;
using System.Linq;

using infrastructure.cqs;
using persistance.dapper.common;
using query.dto;
using query.messages.post;

namespace query.handlers.post
{
    public class GetPostByIdQueryHandler : IQueryHandler<GetPostByIdQuery, PostDTO>/*, IQueryHandler<GetPostByGuidQuery, DTO>*/
    {
        private readonly IDapperConnectionFactory _dapperConnectionFactory;

        public GetPostByIdQueryHandler(IDapperConnectionFactory dapperConnectionFactory)
        {
            _dapperConnectionFactory = dapperConnectionFactory;
        }

        public PostDTO Handle(GetPostByIdQuery query)
        {
            using (var dapper = _dapperConnectionFactory.CreateConnection())
            {
                dapper.Open();
                return dapper.Connection.Query<PostDTO>("select * from posts where id = @Id", new { Id = query.PostId}).FirstOrDefault();
            }
        }
    }
}
