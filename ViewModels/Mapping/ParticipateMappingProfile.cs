using AutoMapper;
using Global.Abstractions.Areas.GiftingGroup;
using ViewModels.Models.Participate;

namespace ViewModels.Mapping;

public sealed class ParticipateMappingProfile : Profile
{
    public ParticipateMappingProfile()
    {
        CreateMap<IManageUserGiftingGroupYear, ManageUserGiftingGroupYearVm>();
        CreateMap<IUserGiftingGroupYear, ManageUserGiftingGroupYearVm>();
    }
}
