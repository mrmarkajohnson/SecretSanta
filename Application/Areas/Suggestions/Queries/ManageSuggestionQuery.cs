using Application.Areas.Suggestions.BaseModels;
using Application.Shared.Requests;
using Global.Abstractions.Areas.Suggestions;
using Global.Extensions.Exceptions;

namespace Application.Areas.Suggestions.Queries;

public class ManageSuggestionQuery : BaseQuery<IManageSuggestion>
{
    private readonly int? _suggestionKey;
    private readonly int? _groupKey;

    public ManageSuggestionQuery(int? suggestionKey, int? groupKey)
    {
        _suggestionKey = suggestionKey;
        _groupKey = groupKey;
    }

    protected override Task<IManageSuggestion> Handle()
    {
        Santa_User dbSantaUser = GetCurrentSantaUser(s => s.GiftingGroupLinks, s => s.Suggestions);
        var dbGiftingGroupLinks = dbSantaUser.GiftingGroupLinks
            .Where(x => x.DateDeleted == null && x.GiftingGroup.DateDeleted == null);

        var suggestion = new ManageSuggestion();

        if (_suggestionKey > 0)
        {
            Santa_Suggestion? dbSuggestion = dbSantaUser.Suggestions
                .FirstOrDefault(x => x.SuggestionKey == _suggestionKey); // we can include deleted and archived here, it shouldn't matter

            if (dbSuggestion != null)
            {
                Mapper.Map(dbSuggestion, suggestion);
            }
            else
            {
                throw new NotFoundException("Suggestion");
            }
        }
        else if (_groupKey > 0)
        {
            Santa_GiftingGroupUser? dbGroupUser = dbGiftingGroupLinks
                .FirstOrDefault(x => x.GiftingGroupKey == _groupKey);

            if (dbGroupUser != null)
            {
                AddGroupLink(suggestion, dbGroupUser, true);
            }
            else
            {
                throw new NotFoundException("Group");
            }
        }

        var dbOtherGroupLinks = dbGiftingGroupLinks
            .Where(x => suggestion.YearGroupUserLinks.Any(y => y.GiftingGroupKey == x.GiftingGroupKey) == false);

        foreach (Santa_GiftingGroupUser dbGroupUser in dbOtherGroupLinks)
        {
            AddGroupLink(suggestion, dbGroupUser, false);
        }

        suggestion.YearGroupUserLinks = suggestion.YearGroupUserLinks.OrderBy(x => x.GiftingGroupName).ToList();

        return Result(suggestion);
    }

    private void AddGroupLink(ManageSuggestion suggestion, Santa_GiftingGroupUser dbGroupUser, bool applyToGroup)
    {
        int calendarYear = DateTime.Today.Year;

        Santa_YearGroupUser? dbYearGroupUser = dbGroupUser.SantaUser.GiftingGroupYears
            .Where(x => x.GiftingGroupYear.DateDeleted == null)
            .FirstOrDefault(x => x.GiftingGroupYear.GiftingGroupKey == dbGroupUser.GiftingGroupKey
                && x.GiftingGroupYear.CalendarYear == calendarYear);

        suggestion.YearGroupUserLinks.Add(new ManageSuggestionLink
        {
            SuggestionLinkKey = 0,
            YearGroupUserKey = dbYearGroupUser?.YearGroupUserKey ?? 0,
            GiftingGroupName = dbGroupUser.GiftingGroup.Name,
            Included = dbYearGroupUser?.Included ?? false,
            GiftingGroupKey = dbGroupUser.GiftingGroupKey,
            CalendarYear = calendarYear,
            ApplyToGroup = applyToGroup
        });
    }
}
