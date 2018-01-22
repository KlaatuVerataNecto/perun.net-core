using System;

using infrastructure.cqs;
using query.dto.common;

namespace query.messages.post
{
    public class GetPostByGuidQuery : IQuery<DTO>
    {
        public Guid Guid { get; set; }
    }
}
