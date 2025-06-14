using AutoMapper;
using Global.Abstractions.Areas.Suggestions;
using ViewModels.Models.Suggestions;

namespace ViewModels.Mapping;

public class SuggestionsMappingProfile : Profile
{
    public SuggestionsMappingProfile()
    {
        CreateMap<IManageSuggestion, ManageSuggestionVm>();
    }
}
