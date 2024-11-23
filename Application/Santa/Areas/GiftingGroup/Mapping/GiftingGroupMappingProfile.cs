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
            .IncludeMembers(src => src.SantaUser.GlobalUser)
            .ForMember(dest => dest.ApplicationId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.GiftingGroup.Name))
            .ForMember(dest => dest.PreviousRequestCount, opt => opt.MapFrom(src => src.SantaUser.GiftingGroupApplications
                .Where(x => x.GiftingGroupId == src.GiftingGroupId && x.Id != src.Id)
                .Count()))
            .ForMember(dest => dest.Accepted, opt => opt.MapFrom(src => src.Accepted))
            .ForMember(dest => dest.RejectionMessage, opt => opt.MapFrom(src => src.RejectionMessage))
            .ForMember(dest => dest.Blocked, opt => opt.MapFrom(src => src.SantaUser.GiftingGroupApplications
                .Where(x => x.GiftingGroupId == src.GiftingGroupId)
                .Any(x => x.Blocked)))
            .ForMember(dest => dest.IdentificationHashed, opt => opt.MapFrom(src => true));
        CreateMap<Santa_GiftingGroupApplication, IReviewApplication>().As<ReviewJoinerApplication>();

        CreateMap<Global_User, ReviewJoinerApplication>()
            .IncludeBase<Global_User, UserNamesBase>()
            .ForMember(dest => dest.ApplicantId, opt => opt.MapFrom(src => src.Id));
        CreateMap<Global_User, IReviewApplication>().As<ReviewJoinerApplication>();

        CreateMap<Santa_GiftingGroupYear, GiftingGroupYear>()
            .IncludeMembers(src => src.GiftingGroup)
            .ForMember(dest => dest.GroupMembers, opt => opt.MapFrom(src => src.Users));
        CreateMap<Santa_GiftingGroupYear, IGiftingGroupYear>().As<GiftingGroupYear>();

        CreateMap<Santa_GiftingGroup, GiftingGroupYear>()
            .ForMember(dest => dest.GiftingGroupId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.GiftingGroupName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.CurrencyCode, opt => opt.MapFrom(src => src.CurrencyCodeOverride))
            .ForMember(dest => dest.CurrencySymbol, opt => opt.MapFrom(src => src.CurrencySymbolOverride));
        CreateMap<Santa_GiftingGroupYear, IGiftingGroupYear>().As<GiftingGroupYear>();

        CreateMap<Santa_YearGroupUser, YearGroupUserBase>()
            .IncludeMembers(src => src.SantaUser);
        CreateMap<Santa_YearGroupUser, IYearGroupUserBase>().As<YearGroupUserBase>();

        CreateMap<Santa_GiftingGroupUser, YearGroupUserBase>()
            .IncludeMembers(src => src.SantaUser)
            .ForMember(dest => dest.Included, opt => opt.MapFrom(src => true));
        CreateMap<Santa_GiftingGroupUser, IYearGroupUserBase>().As<YearGroupUserBase>();

        CreateMap<Santa_User, YearGroupUserBase>()
            .IncludeMembers(src => src.GlobalUser)
            .ForMember(dest => dest.SantaUserId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Included, opt => opt.MapFrom(src => true));
        CreateMap<Santa_User, IYearGroupUserBase>().As<YearGroupUserBase>();

        CreateMap<Global_User, YearGroupUserBase>()
            .IncludeBase<Global_User, UserNamesBase>();
        CreateMap<Global_User, IYearGroupUserBase>().As<YearGroupUserBase>();
    }
}
