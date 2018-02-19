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
            // Command is validated on construction so let's create the domain object
            var post = new Post(command.CommandId ,command.Title, command.ImageName, command.UrlSlug);
            _repositoryPost.Add(post);
        }
    }
}
