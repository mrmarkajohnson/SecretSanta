using Application.Shared.BaseModels;
using AutoMapper;

namespace Application.Shared.Mapping;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<Global_User, UserNamesBase>()
            .ForMember(dest => dest.GlobalUserId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Forename, opt => opt.MapFrom(src => src.Forename))
            .ForMember(dest => dest.MiddleNames, opt => opt.MapFrom(src => src.MiddleNames))
            .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.Surname))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
        CreateMap<Global_User, IUserNamesBase>().As<UserNamesBase>();

        CreateMap<Global_User, IUserAllNames>().As<UserNamesBase>();

        CreateMap<IUserNamesBase, UserNamesBase>();
    }
}
