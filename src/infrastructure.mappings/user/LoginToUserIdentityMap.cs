
namespace infrastructure.mappings.user
{
    public class LoginToUserIdentityMap : Profile
    {
        public LoginToUserIdentityMap()
        {
            CreateMap<AccreditationCardInitial, UserIdentity>()
            .ConstructUsing(x => new UserIdentity
            {
                __OPERATOR__ = x.empty,
                userid = x.userId.ToString(),
                effectivestartdate = DateFormatterService.ConvertOrDefault(x.start_Date, 0),
                startDateOrder = x.start_Date


            });
        }
    }
}
