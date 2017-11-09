using command.messages.post;
using infrastructure.cqs;

namespace command.handlers.post
{
    public class PostCommandHandlers : ICommandHandler<CreatePostCommand>
    {
        //private readonly IRepository<Post> _repositoryPost;

        //public PostCommandHandlers(IRepository<Post> repositoryPost)
        //{
        //    _repositoryPost = repositoryPost;
        //}

        public void Handle(CreatePostCommand command)
        {
            // Save the stuff or throw exception

            //var post = new Post(command.Title);
            //_repositoryPost.Save(post);

        }
    }
}
