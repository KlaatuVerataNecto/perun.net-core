using AutoMapper;
using infrastructure.user.entities;
using infrastructure.user.interfaces;
using infrastructure.user.models;

namespace infrastructure.user.mappings
{
    public class LoginToUserLoginMap : Profile
    {
        public LoginToUserLoginMap(IAuthSchemeNameService authSchemeNameService)
        {
            CreateMap<LoginDb, UserLogin>()
            .ConstructUsing(x => new UserLogin
            (
                x.User.id,
                x.User.username,
                x.email,
                x.provider,
                authSchemeNameService.getDefaultProvider()
            ));
        }
    }
}
