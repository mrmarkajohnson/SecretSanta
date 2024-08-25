using Application.Santa.Areas.GiftingGroup.BaseModels;
using AutoMapper;
using Global.Abstractions.Santa.Areas.GiftingGroup;

namespace Application.Santa.Areas.GiftingGroup.Mapping;

public class GiftingGroupMappingProfile : Profile
{
	public GiftingGroupMappingProfile()
	{
        CreateMap<Santa_GiftingGroup, CoreGiftingGroup>();
        CreateMap<Santa_GiftingGroup, IGiftingGroup>().As<CoreGiftingGroup>();
	}
}
