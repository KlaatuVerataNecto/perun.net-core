using infrastructure.cqs;
using query.dto;

namespace query.messages.post
{
    public class GetPostByIdQuery : IQuery<PostDTO>
    {
        public int PostId { get; set; }
    }
}
