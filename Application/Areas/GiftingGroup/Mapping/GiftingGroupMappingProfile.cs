using Application.Areas.GiftingGroup.BaseModels;
using Application.Shared.BaseModels;
using AutoMapper;
using Global.Abstractions.Areas.GiftingGroup;

namespace Application.Areas.GiftingGroup.Mapping;

public class GiftingGroupMappingProfile : Profile
{
    public GiftingGroupMappingProfile()
    {
        CreateMap<Santa_GiftingGroup, CoreGiftingGroup>();
        CreateMap<Santa_GiftingGroup, IGiftingGroup>().As<CoreGiftingGroup>();

        CreateMap<Santa_GiftingGroupUser, UserGiftingGroupYear>()
            .ForMember(dest => dest.GiftingGroupId, opt => opt.MapFrom(src => src.GiftingGroupId))
            .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.GiftingGroup.Name))
            .ForMember(dest => dest.GroupAdmin, opt => opt.MapFrom(src => src.GroupAdmin))
            .ForMember(dest => dest.Included, opt => opt.Ignore())
            .ForMember(dest => dest.Recipient, opt => opt.Ignore());
        CreateMap<Santa_GiftingGroupUser, IUserGiftingGroupYear>().As<UserGiftingGroupYear>();

        CreateMap<Santa_GiftingGroupUser, UserGiftingGroup>()
            .ForMember(dest => dest.GiftingGroupId, opt => opt.MapFrom(src => src.GiftingGroupId))
            .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.GiftingGroup.Name))
            .ForMember(dest => dest.GroupAdmin, opt => opt.MapFrom(src => src.GroupAdmin))
            .ForMember(dest => dest.NewApplications, opt => opt.MapFrom(src => 
                src.GroupAdmin ? src.GiftingGroup.MemberApplications.Where(x => !x.Blocked && x.ResponseByUserId == null).Count() : 0));
        CreateMap<Santa_GiftingGroupUser, IUserGiftingGroup>().As<UserGiftingGroup>();

        CreateMap<Santa_GiftingGroupApplication, ReviewJoinerApplication>()
            .IncludeMembers(src => src.SantaUser.GlobalUser)
            .ForMember(dest => dest.ApplicationId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.GiftingGroup.Name))
            .ForMember(dest => dest.PreviousRequestCount, opt => opt.MapFrom(src => src.SantaUser.GiftingGroupApplications
                .Where(x => x.GiftingGroupId == src.GiftingGroupId && x.Id != src.Id)
                .Count()))
            .ForMember(dest => dest.CurrentYearCalculated, opt => opt.MapFrom(src => src.GiftingGroup.Years.Any(x => x.Year == DateTime.Today.Year)
                ? src.GiftingGroup.Years.First(x => x.Year == DateTime.Today.Year).Users.Any(x => x.GivingToUserId != null)
                : false))
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
            .ForMember(dest => dest.GroupMembers, opt => opt.MapFrom(src => src.Users))
            .ForMember(dest => dest.Calculated, opt => opt.MapFrom(src => src.Users.Any(x => x.GivingToUserId != null)))
            .ForMember(dest => dest.RecalculationRequired, opt => opt.MapFrom(src => src.Users.Any(x => x.GivingToUserId != null)
                && src.Users.Any(x => x.Included == true && x.GivingToUserId == null)));
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
            .ForMember(dest => dest.Included, opt => opt.Ignore());
        CreateMap<Santa_GiftingGroupUser, IYearGroupUserBase>().As<YearGroupUserBase>();

        CreateMap<Santa_User, YearGroupUserBase>()
            .IncludeMembers(src => src.GlobalUser)
            .ForMember(dest => dest.SantaUserId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Included, opt => opt.Ignore());
        CreateMap<Santa_User, IYearGroupUserBase>().As<YearGroupUserBase>();

        CreateMap<Global_User, YearGroupUserBase>()
            .IncludeBase<Global_User, UserNamesBase>();
        CreateMap<Global_User, IYearGroupUserBase>().As<YearGroupUserBase>();

        CreateMap<IYearGroupUserBase, YearGroupUserBase>();
    }
}
