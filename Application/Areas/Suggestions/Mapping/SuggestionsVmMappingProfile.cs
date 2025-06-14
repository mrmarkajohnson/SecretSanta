using Application.Areas.Suggestions.ViewModels;
using AutoMapper;
using Global.Abstractions.Areas.Suggestions;

namespace Application.Areas.Suggestions.Mapping;

public class SuggestionsVmMappingProfile : Profile
{
    public SuggestionsVmMappingProfile()
    {
        CreateMap<IManageSuggestion, ManageSuggestionVm>();
    }
}
