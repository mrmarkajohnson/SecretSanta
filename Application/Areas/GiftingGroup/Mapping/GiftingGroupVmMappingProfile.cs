using Application.Areas.GiftingGroup.ViewModels;
using Application.Areas.Participate.ViewModels;
using AutoMapper;
using Global.Abstractions.Areas.GiftingGroup;
using Global.Abstractions.Areas.Participate;

namespace Application.Areas.GiftingGroup.Mapping;

public sealed class GiftingGroupVmMappingProfile : Profile
{
    public GiftingGroupVmMappingProfile()
    {
        CreateMap<IGiftingGroup, EditGiftingGroupVm>();
        CreateMap<IReviewApplication, ReviewJoinerApplicationVm>();
        CreateMap<IGiftingGroupYear, SetupGiftingGroupYearVm>();
        CreateMap<IUserGiftingGroupYear, ManageUserGiftingGroupYearVm>();
    }
}
