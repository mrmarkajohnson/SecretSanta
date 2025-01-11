using Application.Areas.Account.BaseModels;
using AutoMapper;

namespace Application.Areas.Account.Mapping;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<Global_User, SantaUser>()
            .ForMember(dest => dest.IdentificationHashed, opt => opt.MapFrom(src => true));
    }
}
