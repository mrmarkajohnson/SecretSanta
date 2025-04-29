using AutoMapper;
using Global.Abstractions.Areas.Suggestions;
using ViewLayer.Models.Suggestions;

namespace ViewLayer.Mapping;

public class SuggestionsMappingProfile : Profile
{
    public SuggestionsMappingProfile()
    {
        CreateMap<IManageSuggestion, ManageSuggestionVm>();
    }
}
