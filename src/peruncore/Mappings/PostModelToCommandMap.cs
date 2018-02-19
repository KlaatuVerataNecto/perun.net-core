using System;
using AutoMapper;

using command.messages.post;
using infrastucture.libs.strings;
using peruncore.Models.Post;

namespace peruncore.Mappings
{
    public class PostModelToCommandMap : Profile
    {
        public PostModelToCommandMap()
        {
            CreateMap<PostModel,CreatePostCommand>()
            .ConstructUsing(x => new CreatePostCommand
            (
                Guid.NewGuid(),
                x.title,
                x.post_image,
                StringService.GenerateSlug(x.title)
            ));
        }
    }
}
