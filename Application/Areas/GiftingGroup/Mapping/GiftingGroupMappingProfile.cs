using Application.Areas.GiftingGroup.BaseModels;
using Application.Shared.BaseModels;
using AutoMapper;
using Global.Abstractions.Areas.GiftingGroup;
using static Global.Settings.GiftingGroupSettings;

namespace Application.Areas.GiftingGroup.Mapping;

public sealed class GiftingGroupMappingProfile : Profile
{
    public GiftingGroupMappingProfile()
    {
        CreateMap<Santa_GiftingGroup, CoreGiftingGroup>()
            .ForMember(x => x.FirstYear, opt => opt.MapFrom(src => src.FirstYear == 0 && src.DateCreated.Year < DateTime.Now.Year 
                ? src.DateCreated.Year
                : src.FirstYear));
        CreateMap<Santa_GiftingGroup, IGiftingGroup>().As<CoreGiftingGroup>();

        CreateMap<Santa_GiftingGroupUser, UserGiftingGroup>()
            .ForMember(dest => dest.GiftingGroupKey, opt => opt.MapFrom(src => src.GiftingGroupKey))
            .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.GiftingGroup.Name))
            .ForMember(dest => dest.GroupAdmin, opt => opt.MapFrom(src => src.GroupAdmin))
            .ForMember(dest => dest.NewApplications, opt => opt.MapFrom(src =>
                src.GroupAdmin ? src.GiftingGroup.MemberApplications
                    .Where(x => x.DateArchived == null)
                    .Where(x => x.SantaUser.DateArchived == null)
                    .Count(x => !x.Blocked && x.ResponseBySantaUserKey == null) : 0))
            .ForMember(dest => dest.CurrentYear, opt => opt.MapFrom(src => 
                src.DateCreated.Year > GlobalSettings.CurrentYear ? src.DateCreated.Year : GlobalSettings.CurrentYear));
        CreateMap<Santa_GiftingGroupUser, IUserGiftingGroup>().As<UserGiftingGroup>();

        CreateMap<Santa_GiftingGroupApplication, ReviewJoinerApplication>()
            .IncludeMembers(src => src.SantaUser.GlobalUser)
            .ForMember(dest => dest.GroupApplicationKey, opt => opt.MapFrom(src => src.GroupApplicationKey))
            .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.GiftingGroup.Name))
            .ForMember(dest => dest.ApplicantMessage, opt => opt.MapFrom(src => src.Message))
            .ForMember(dest => dest.PreviousRequestCount, opt => opt.MapFrom(src => src.SantaUser.GiftingGroupApplications
                .Where(x => x.GiftingGroupKey == src.GiftingGroupKey && x.GroupApplicationKey != src.GroupApplicationKey)
                .Count()))
            .ForMember(dest => dest.CurrentYearCalculated, opt => opt.MapFrom(src => src.GiftingGroup.Years.Any(x => x.CalendarYear >= GlobalSettings.CurrentYear)
                ? src.GiftingGroup.Years.First(x => x.CalendarYear >= GlobalSettings.CurrentYear).Users.Any(x => x.RecipientSantaUserKey != null)
                : false))
            .ForMember(dest => dest.Accepted, opt => opt.MapFrom(src => src.Accepted))
            .ForMember(dest => dest.RejectionMessage, opt => opt.MapFrom(src => src.RejectionMessage))
            .ForMember(dest => dest.Blocked, opt => opt.MapFrom(src => src.SantaUser.GiftingGroupApplications
                .Where(x => x.GiftingGroupKey == src.GiftingGroupKey)
                .Any(x => x.Blocked)))
            .ForMember(dest => dest.IdentificationHashed, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.ShowEmail, opt => opt.MapFrom(src => true));
        CreateMap<Santa_GiftingGroupApplication, IReviewApplication>().As<ReviewJoinerApplication>();

        CreateMap<Global_User, ReviewJoinerApplication>()
            .IncludeBase<Global_User, UserNamesBase>()
            .ForMember(dest => dest.ShowEmail, opt => opt.MapFrom(src => true));
        CreateMap<Global_User, IReviewApplication>().As<ReviewJoinerApplication>();

        CreateMap<Santa_GiftingGroupYear, GiftingGroupYear>()
            .IncludeMembers(src => src.GiftingGroup)
            .ForMember(dest => dest.Limit, opt => opt.MapFrom(dest => dest.Limit))
            .ForMember(dest => dest.CurrencyCode, opt => opt.MapFrom(src => src.CurrencyCode))
            .ForMember(dest => dest.CurrencySymbol, opt => opt.MapFrom(src => src.CurrencySymbol))
            .ForMember(dest => dest.GroupMembers, opt => opt.MapFrom(src => src.Users
                .Where(x => x.SantaUser.DateArchived == null)
                .Where(x => x.SantaUser.GiftingGroupLinks.Any(x => x.GiftingGroupKey == src.GiftingGroupKey && x.DateArchived == null))))
            .ForMember(dest => dest.Calculated, opt => opt.MapFrom(src => src.Users.Any(x => x.RecipientSantaUserKey != null)))
            .ForMember(dest => dest.RecalculationRequired, opt => opt.MapFrom(src => src.Users.Any(x => x.RecipientSantaUserKey != null)
                && src.Users.Any(x => x.Included == true && x.RecipientSantaUserKey == null 
                && x.SantaUser.GiftingGroupLinks.Any(y => y.GiftingGroupKey == src.GiftingGroupKey 
                && y.DateArchived == null
                && y.SantaUser.DateArchived == null))));
        CreateMap<Santa_GiftingGroupYear, IGiftingGroupYear>().As<GiftingGroupYear>();

        CreateMap<Santa_GiftingGroup, GiftingGroupYear>()
            .ForMember(dest => dest.GiftingGroupKey, opt => opt.MapFrom(src => src.GiftingGroupKey))
            .ForMember(dest => dest.GiftingGroupName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.CurrencyCode, opt => opt.MapFrom(src => src.CurrencyCodeOverride))
            .ForMember(dest => dest.CurrencySymbol, opt => opt.MapFrom(src => src.CurrencySymbolOverride));
        CreateMap<Santa_GiftingGroupYear, IGiftingGroupYear>().As<GiftingGroupYear>();

        CreateMap<Santa_YearGroupUser, YearGroupUser>()
            .IncludeMembers(src => src.SantaUser)
            .ForMember(dest => dest.Included, opt => opt.MapFrom(src => src.Included))
            .ForMember(dest => dest.Suggestions, opt => opt.MapFrom(src => src.Suggestions
                .Where(x => x.DateDeleted == null && x.DateArchived == null)
                .Count()));
        CreateMap<Santa_YearGroupUser, IYearGroupUser>().As<YearGroupUser>();

        CreateMap<Santa_GiftingGroupUser, YearGroupUser>()
            .IncludeMembers(src => src.SantaUser)
            .ForMember(dest => dest.Included, opt => opt.Ignore())
            .ForMember(dest => dest.Suggestions, opt => opt.Ignore());
        CreateMap<Santa_GiftingGroupUser, IYearGroupUser>().As<YearGroupUser>();

        CreateMap<Santa_User, YearGroupUser>()
            .IncludeMembers(src => src.GlobalUser)
            .ForMember(dest => dest.SantaUserKey, opt => opt.MapFrom(src => src.SantaUserKey))
            .ForMember(dest => dest.Included, opt => opt.Ignore())
            .ForMember(dest => dest.Suggestions, opt => opt.Ignore());
        CreateMap<Santa_User, IYearGroupUser>().As<YearGroupUser>();

        CreateMap<Global_User, YearGroupUser>()
            .IncludeBase<Global_User, UserNamesBase>();
        CreateMap<Global_User, IYearGroupUser>().As<YearGroupUser>();

        CreateMap<IYearGroupUser, YearGroupUser>();

        CreateMap<Santa_GiftingGroupUser, GroupMember>()
            .IncludeMembers(src => src.SantaUser)
            .ForMember(dest => dest.MemberStatus, opt => opt.MapFrom(src => src.GroupAdmin ? GroupMemberStatus.Admin : GroupMemberStatus.Joined));
        CreateMap<Santa_GiftingGroupUser, IGroupMember>().As<GroupMember>();

        CreateMap<Santa_Invitation, GroupMember>()
            .IncludeMembers(src => src.ToSantaUser)
            .ForMember(dest => dest.MemberStatus, opt => opt.MapFrom(src => GroupMemberStatus.Invited));
        CreateMap<Santa_Invitation, IGroupMember>().As<GroupMember>();

        CreateMap<Santa_GiftingGroupApplication, GroupMember>()
            .IncludeMembers(src => src.SantaUser)
            .ForMember(dest => dest.MemberStatus, opt => opt.MapFrom(src => GroupMemberStatus.Applied))
            .ForMember(dest => dest.GroupApplicationKey, opt => opt.MapFrom(src => src.GroupApplicationKey));
        CreateMap<Santa_GiftingGroupApplication, IGroupMember>().As<GroupMember>();

        CreateMap<Santa_User, GroupMember>()
            .IncludeBase<Santa_User, UserNamesBase>()
            .ForMember(dest => dest.SantaUserKey, opt => opt.MapFrom(src => src.SantaUserKey));
        CreateMap<Santa_GiftingGroupUser, IGroupMember>().As<GroupMember>();

        CreateMap<Santa_Invitation, ReviewGroupInvitation>()
            .ForMember(dest => dest.InvitationGuid, opt => opt.MapFrom(src => src.InvitationGuid))
            .ForMember(dest => dest.GiftingGroupKey, opt => opt.MapFrom(src => src.GiftingGroupKey))
            .ForMember(dest => dest.ToSantaUserKey,opt => opt.MapFrom(src => src.ToSantaUserKey))
            .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.GiftingGroup.Name))
            .ForMember(dest => dest.GroupDescription, opt => opt.MapFrom(src => src.GiftingGroup.Description))
            .ForMember(dest => dest.FromUser, opt => opt.MapFrom(src => src.FromSantaUser))
            .ForMember(dest => dest.InvitationMessage, opt => opt.MapFrom(src => src.InvitationMessage));
        CreateMap<Santa_Invitation, IReviewGroupInvitation>().As<ReviewGroupInvitation>();
    }
}
