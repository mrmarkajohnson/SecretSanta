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

        CreateMap<Santa_SuggestionLink, SuggestionYearGroupUserLink>()
            .IncludeMembers(src => src.YearGroupUser)
            .ForMember(dest => dest.SuggestionLinkKey, opt => opt.MapFrom(src => src.SuggestionLinkKey));
        CreateMap<Santa_SuggestionLink, ISuggestionYearGroupUserLink>().As<SuggestionYearGroupUserLink>();

        CreateMap<Santa_YearGroupUser, SuggestionYearGroupUserLink>()
            .ForMember(dest => dest.YearGroupUserKey, opt => opt.MapFrom(src => src.YearGroupUserKey))
            .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.GiftingGroupYear.Year))
            .ForMember(dest => dest.GiftingGroupName, opt => opt.MapFrom(src => src.GiftingGroupYear.GiftingGroup.Name));
    }
}
