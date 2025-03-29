using AutoMapper;
using Global.Abstractions.Areas.GiftingGroup;
using ViewLayer.Models.Participate;

namespace ViewLayer.Mapping;

public class ParticipateMappingProfile : Profile
{
	public ParticipateMappingProfile()
	{
        CreateMap<IManageUserGiftingGroupYear, ManageUserGiftingGroupYearVm>();
        CreateMap<IUserGiftingGroupYear, ManageUserGiftingGroupYearVm>();
	}
}
