using AutoMapper;
using infrastructure.user.entities;
using infrastructure.user.models;

namespace infrastructure.user.mappings
{
    public class UsertoUserProfileMap : Profile
    {
        public UsertoUserProfileMap()
        {
            CreateMap<UserDb, UserProfile>()
            .ConstructUsing(x => new UserProfile
            (
                x.id,
                x.username, 
                x.avatar,
                x.cover
            ));
        }
    }
}
