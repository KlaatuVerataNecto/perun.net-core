using Dapper;
using System.Linq;

using infrastructure.cqs;
using persistance.dapper.common;
using query.dto.common;
using query.messages.post;


namespace query.handlers.post
{
    public class GetDTOByGuidQueryHandler : IQueryHandler<GetPostByGuidQuery, DTO>
    {
        private readonly IDapperConnectionFactory _dapperConnectionFactory;

        public GetDTOByGuidQueryHandler(IDapperConnectionFactory dapperConnectionFactory)
        {
            _dapperConnectionFactory = dapperConnectionFactory;
        }

        public DTO Handle(GetPostByGuidQuery query)
        {
            using (var dapper = _dapperConnectionFactory.CreateConnection())
            {
                dapper.Open();
                return dapper.Connection.Query<DTO>("select * from posts where guid = @Guid", new { Guid = query.Guid }).FirstOrDefault();
            }
        }
    }
}
