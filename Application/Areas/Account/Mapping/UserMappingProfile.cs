using Application.Areas.Account.BaseModels;
using Application.Shared.BaseModels;
using AutoMapper;

namespace Application.Areas.Account.Mapping;

#pragma warning disable CS8602 // Dereference of a possibly null reference.

public sealed class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        IList<int> UserKeysForVisibleEmail = new List<int>();

        CreateMap<Global_User, SantaUser>()
            .ForMember(dest => dest.IdentificationHashed, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.ShowEmail, opt => opt.MapFrom(src => UserKeysForVisibleEmail.Contains(src.SantaUser.SantaUserKey)));

        CreateMap<Santa_User, UserNamesBase>()
            .IncludeMembers(src => src.GlobalUser);
        CreateMap<Santa_User, IUserNamesBase>().As<UserNamesBase>();

        CreateMap<Santa_User, IUserAllNames>().As<UserNamesBase>();
    }
}
