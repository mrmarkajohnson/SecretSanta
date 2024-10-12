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

        CreateMap<Santa_GiftingGroupApplication, ReviewJoinerApplication>()
            .IncludeMembers(src => src.User.GlobalUser)
            .ForMember(dest => dest.ApplicationId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.GiftingGroup.Name))
            .ForMember(dest => dest.PreviousRequestCount, opt => opt.MapFrom(src => src.User.GiftingGroupApplications
                .Where(x => x.GiftingGroupId == src.GiftingGroupId && x.Id != src.Id)
                .Count()))
            .ForMember(dest => dest.Accepted, opt => opt.MapFrom(src => src.Accepted))
            .ForMember(dest => dest.RejectionMessage, opt => opt.MapFrom(src => src.RejectionMessage))
            .ForMember(dest => dest.Blocked, opt => opt.MapFrom(src => src.User.GiftingGroupApplications
                .Where(x => x.GiftingGroupId == src.GiftingGroupId)
                .Any(x => x.Blocked)))
            .ForMember(dest => dest.IdentificationHashed, opt => opt.MapFrom(src => true));

        CreateMap<Global_User, ReviewJoinerApplication>()
            .ForMember(dest => dest.ApplicantId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Forename + " " + src.Surname))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

        CreateMap<Santa_GiftingGroupApplication, IReviewApplication>().As<ReviewJoinerApplication>();
	}
}
