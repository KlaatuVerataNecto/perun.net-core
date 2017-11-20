using infrastructure.cqs;
using persistance.dapper.common;
using query.dto;
using query.messages.post;

namespace query.handlers.post
{
    public class GetPostByIdQueryHandler : IQueryHandler<GetPostByIdQuery, PostDTO>
    {
        private readonly IDapperConnectionFactory _dapperConnectionFactory;

        public GetPostByIdQueryHandler(IDapperConnectionFactory dapperConnectionFactory)
        {
            _dapperConnectionFactory = dapperConnectionFactory;
        }

        public PostDTO Handle(GetPostByIdQuery query)
        {
            throw new System.NotImplementedException();
        }
    }
}
