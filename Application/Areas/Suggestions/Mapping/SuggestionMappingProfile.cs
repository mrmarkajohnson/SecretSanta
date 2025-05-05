using Application.Areas.Suggestions.BaseModels;
using AutoMapper;
using Global.Abstractions.Areas.Suggestions;

namespace Application.Areas.Suggestions.Mapping;

public class SuggestionMappingProfile : Profile
{
    public SuggestionMappingProfile()
    {
        CreateMap<Santa_Suggestion, SuggestionBase>()
            .ForMember(dest => dest.SuggestionKey, opt => opt.MapFrom(src => src.SuggestionKey))
            .ForMember(dest => dest.SantaUserKey, opt => opt.MapFrom(src => src.SantaUserKey))
            .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority))
            .ForMember(dest => dest.SuggestionText, opt => opt.MapFrom(src => src.SuggestionText))
            .ForMember(dest => dest.OtherNotes, opt => opt.MapFrom(src => src.OtherNotes));
        CreateMap<Santa_Suggestion, ISuggestionBase>().As<SuggestionBase>();

        CreateMap<Santa_Suggestion, Suggestion>()
            .IncludeBase<Santa_Suggestion, SuggestionBase>()
            .ForMember(dest => dest.YearGroupUserLinks, opt => opt.MapFrom(src => src.YearGroupUserLinks));
        CreateMap<Santa_Suggestion, ISuggestion>().As<Suggestion>();

        CreateMap<Santa_Suggestion, ManageSuggestion>()
            .IncludeBase<Santa_Suggestion, SuggestionBase>()
            .ForMember(dest => dest.YearGroupUserLinks, opt => opt.MapFrom(src => src.YearGroupUserLinks));
        CreateMap<Santa_Suggestion, IManageSuggestion>().As<ManageSuggestion>();

        CreateMap<Santa_SuggestionLink, SuggestionYearGroupUserLink>()
            .IncludeMembers(src => src.YearGroupUser)
            .ForMember(dest => dest.SuggestionLinkKey, opt => opt.MapFrom(src => src.SuggestionLinkKey));
        CreateMap<Santa_SuggestionLink, ISuggestionYearGroupUserLink>().As<SuggestionYearGroupUserLink>();

        CreateMap<Santa_SuggestionLink, ManageSuggestionLink>()
            .IncludeMembers(src => src.YearGroupUser)
            .IncludeBase<Santa_SuggestionLink, SuggestionYearGroupUserLink>()
            .ForMember(dest => dest.ApplyToGroup, opt => opt.MapFrom(src => src.DateDeleted == null && src.DateArchived == null
                && src.Suggestion.DateDeleted == null));
        CreateMap<Santa_SuggestionLink, IManageSuggestionLink>().As<ManageSuggestionLink>();

        CreateMap<Santa_YearGroupUser, SuggestionYearGroupUserLink>()
            .ForMember(dest => dest.YearGroupUserKey, opt => opt.MapFrom(src => src.YearGroupUserKey))
            .ForMember(dest => dest.GiftingGroupName, opt => opt.MapFrom(src => src.GiftingGroupYear.GiftingGroup.Name));
        CreateMap<Santa_YearGroupUser, ISuggestionYearGroupUserLink>().As<SuggestionYearGroupUserLink>();

        CreateMap<Santa_YearGroupUser, ManageSuggestionLink>()
            .IncludeBase<Santa_YearGroupUser, SuggestionYearGroupUserLink>()
            .ForMember(dest => dest.CalendarYear, opt => opt.MapFrom(src => src.GiftingGroupYear.CalendarYear))
            .ForMember(dest => dest.Included, opt => opt.MapFrom(src => src.Included))
            .ForMember(dest => dest.GiftingGroupKey, opt => opt.MapFrom(src => src.GiftingGroupYear.GiftingGroupKey));
        CreateMap<Santa_YearGroupUser, IManageSuggestionLink>().As<ManageSuggestionLink>();
    }
}
