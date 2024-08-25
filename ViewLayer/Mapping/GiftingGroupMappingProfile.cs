using AutoMapper;
using Global.Abstractions.Santa.Areas.GiftingGroup;
using ViewLayer.Models.GiftingGroup;

namespace ViewLayer.Mapping;

public class GiftingGroupMappingProfile : Profile
{
    public GiftingGroupMappingProfile()
    {
        CreateMap<IGiftingGroup, EditGiftingGroupVm>();
    }
}
