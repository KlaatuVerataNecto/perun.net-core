using Dapper;
using infrastructure.cqs;
using persistance.dapper.common;
using query.dto;
using query.messages.post;
using System.Collections.Generic;
using System.Linq;

namespace query.handlers.post
{
    public class GetAllPublishedPostsQueryHandler : IQueryHandler<GetAllPublishedPostsQuery, List<PostDTO>>
    {
        private readonly IDapperConnectionFactory _dapperConnectionFactory;

        public GetAllPublishedPostsQueryHandler(IDapperConnectionFactory dapperConnectionFactory)
        {
            _dapperConnectionFactory = dapperConnectionFactory;
        }

        public List<PostDTO> Handle(GetAllPublishedPostsQuery query)
        {
            using (var dapper = _dapperConnectionFactory.CreateConnection())
            {
                dapper.Open();
                return dapper.Connection.Query<PostDTO>("select * from posts").ToList();
            }
        }
    }
}
