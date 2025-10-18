using Application.Areas.GiftingGroup.ViewModels;
using Application.Areas.Participate.ViewModels;
using AutoMapper;
using Global.Abstractions.Areas.GiftingGroup;
using Global.Abstractions.Areas.Participate;

namespace Application.Areas.Participate.Mapping;

public sealed class ParticipateVmMappingProfile : Profile
{
    public ParticipateVmMappingProfile()
    {
        CreateMap<IManageUserGiftingGroupYear, ManageUserGiftingGroupYearVm>();
        CreateMap<IUserGiftingGroupYear, ManageUserGiftingGroupYearVm>();
        CreateMap<IReviewApplication, ReviewGroupInvitationVm>();
    }
}
