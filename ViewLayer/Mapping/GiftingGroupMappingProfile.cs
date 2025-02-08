using AutoMapper;
using Global.Abstractions.Areas.GiftingGroup;
using ViewLayer.Models.GiftingGroup;

namespace ViewLayer.Mapping;

public class GiftingGroupMappingProfile : Profile
{
    public GiftingGroupMappingProfile()
    {
        CreateMap<IGiftingGroup, EditGiftingGroupVm>();
        CreateMap<IReviewApplication, ReviewJoinerApplicationVm>();
        CreateMap<IGiftingGroupYear, SetupGiftingGroupYearVm>();
        CreateMap<IUserGiftingGroupYear, UserGiftingGroupYearVm>();
    }
}
