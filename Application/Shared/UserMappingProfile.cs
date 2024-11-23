using Application.Santa.Areas.GiftingGroup.BaseModels;
using AutoMapper;
using Global.Abstractions.Global.Shared;

namespace Application.Shared;

public class UserMappingProfile : Profile
{
	public UserMappingProfile()
	{
        CreateMap<Global_User, UserNamesBase>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Forename, opt => opt.MapFrom(src => src.Forename))
            .ForMember(dest => dest.MiddleNames, opt => opt.MapFrom(src => src.MiddleNames))
            .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.Surname))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
        CreateMap<Global_User, IUserAllNames>().As<UserNamesBase>();
    }
}
