using Application.Shared.BaseModels;
using AutoMapper;

namespace Application.Shared.Mapping;

#pragma warning disable CS8602 // Dereference of a possibly null reference.

public sealed class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        IList<int> UserKeysForVisibleEmail = new List<int>();


        CreateMap<Global_User, UserNamesBase>()
            .ForMember(dest => dest.GlobalUserId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Forename, opt => opt.MapFrom(src => src.Forename))
            .ForMember(dest => dest.MiddleNames, opt => opt.MapFrom(src => src.MiddleNames))
            .ForMember(dest => dest.PreferredNameType, opt => opt.MapFrom(src => src.PreferredNameType))
            .ForMember(dest => dest.PreferredFirstName, opt => opt.MapFrom(src => src.PreferredFirstName))
            .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.Surname))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.ShowEmail, opt => opt.MapFrom(src => UserKeysForVisibleEmail.Contains(src.SantaUser.SantaUserKey)));

        CreateMap<Global_User, IUserNamesBase>().As<UserNamesBase>();

        CreateMap<Global_User, IUserAllNames>().As<UserNamesBase>();

        CreateMap<IUserNamesBase, UserNamesBase>();
    }
}
