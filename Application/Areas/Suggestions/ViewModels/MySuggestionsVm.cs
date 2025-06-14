using Global.Abstractions.Areas.GiftingGroup;
using Global.Abstractions.Areas.Suggestions;

namespace Application.Areas.Suggestions.ViewModels;

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
