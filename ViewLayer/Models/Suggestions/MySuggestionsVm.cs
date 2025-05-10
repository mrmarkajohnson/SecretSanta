using Application.Areas.Suggestions.BaseModels;
using Global.Abstractions.Areas.GiftingGroup;
using Global.Abstractions.Areas.Suggestions;

namespace ViewLayer.Models.Suggestions;

public class MySuggestionsVm
{
    public MySuggestionsVm(IQueryable<ISuggestion> suggestions, IList<IUserGiftingGroup> allGroups)
    {
        Suggestions = suggestions;
        AllGroups = allGroups;
    }

    public IQueryable<ISuggestion> Suggestions { get; set; }
    public IList<IUserGiftingGroup> AllGroups { get; set; }
}
