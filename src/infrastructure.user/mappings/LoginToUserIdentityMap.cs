using AutoMapper;
using infrastructure.user.entities;
using infrastructure.user.models;

namespace infrastructure.user.mappings
{
    public class LoginToUserIdentityMap: Profile
    {
        public LoginToUserIdentityMap()
        {
            CreateMap<LoginDb, UserIdentity>()
            .ConstructUsing(x => new UserIdentity
            (
              x.user_id,
              x.id,
              x.User.username,
              x.email,
              x.provider,
              x.User.roles,
              x.User.avatar              
            ));
        }
    }
}
