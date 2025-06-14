using AutoMapper;
using Global.Abstractions.Areas.GiftingGroup;
using ViewModels.Models.GiftingGroup;
using ViewModels.Models.Participate;

namespace ViewModels.Mapping;

public sealed class GiftingGroupMappingProfile : Profile
{
    public GiftingGroupMappingProfile()
    {
        CreateMap<IGiftingGroup, EditGiftingGroupVm>();
        CreateMap<IReviewApplication, ReviewJoinerApplicationVm>();
        CreateMap<IGiftingGroupYear, SetupGiftingGroupYearVm>();
        CreateMap<IUserGiftingGroupYear, ManageUserGiftingGroupYearVm>();
    }
}
