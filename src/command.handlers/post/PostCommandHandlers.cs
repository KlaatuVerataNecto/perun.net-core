using command.messages.post;
using domain.model;
using infrastructure.cqs;
using persistance.ef.common;

namespace command.handlers.post
{
    public class PostCommandHandlers : ICommandHandler<CreatePostCommand>
    {
        private readonly IRepository<Post> _repositoryPost;

        public PostCommandHandlers(IRepository<Post> repositoryPost)
        {
            _repositoryPost = repositoryPost;
        }

        public void Handle(CreatePostCommand command)
        {
            // TODO: Validate command
            var post = new Post(command.Title);
            _repositoryPost.Add(post);
        }
    }
}
