using Application.Areas.Account.BaseModels;
using Application.Shared.BaseModels;
using AutoMapper;

namespace Application.Areas.Account.Mapping;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<Global_User, SantaUser>()
            .ForMember(dest => dest.IdentificationHashed, opt => opt.MapFrom(src => true));

        CreateMap<Santa_User, UserNamesBase>()
            .IncludeMembers(src => src.GlobalUser);
        CreateMap<Santa_User, IUserNamesBase>().As<UserNamesBase>();

        CreateMap<Santa_User, IUserAllNames>().As<UserNamesBase>();
    }
}
