using infrastructure.cqs;
using query.dto;
using System.Collections.Generic;

namespace query.messages.post
{
    public class GetAllPublishedPostsQuery : IQuery<List<PostDTO>>
    {
        public bool isPublished { get; set; }
    }
}
