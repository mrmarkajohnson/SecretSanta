using Application.Areas.GiftingGroup.Commands;
using Application.Shared.Requests;
using Global.Abstractions.Areas.Suggestions;
using Global.Extensions.Exceptions;

namespace Application.Areas.Suggestions.Commands;

public class SaveSuggestionCommand<TItem> : GiftingGroupYearBaseCommand<TItem> where TItem : class, IManageSuggestion
{
    public SaveSuggestionCommand(TItem item) : base(item)
    {
    }

    protected async override Task<ICommandResult<TItem>> HandlePostValidation()
    {
        Santa_User dbSantaUser = GetCurrentSantaUser(s => s.GiftingGroupLinks, s => s.Suggestions);
        Santa_Suggestion? dbSuggestion = null;

        if (Item.SuggestionKey > 0)
        {
            dbSuggestion = dbSantaUser.Suggestions.FirstOrDefault(x => x.SuggestionKey == Item.SuggestionKey);
        }
        else
        {
            dbSuggestion = new Santa_Suggestion
            {
                SantaUserKey = dbSantaUser.SantaUserKey,
                SantaUser = dbSantaUser
            };

            dbSantaUser.Suggestions.Add(dbSuggestion);
        }

        if (dbSuggestion == null)
        {
            throw new NotFoundException("Suggestion");
        }

        dbSuggestion.SuggestionText = Item.SuggestionText;
        dbSuggestion.OtherNotes = Item.OtherNotes;
        dbSuggestion.Priority = Item.Priority;

        HandleGroupLinks(dbSantaUser, dbSuggestion);

        var result = await SaveAndReturnSuccess();
        Mapper.Map(dbSuggestion, Item); // get any new keys
        return result;
    }

    private void HandleGroupLinks(Santa_User dbSantaUser, Santa_Suggestion dbSuggestion)
    {
        foreach (IManageSuggestionLink link in Item.YearGroupUserLinks)
        {
            Santa_SuggestionLink? dbLink = null;

            if (link.SuggestionLinkKey > 0)
            {
                dbLink = dbSuggestion.YearGroupUserLinks.FirstOrDefault(x => x.SuggestionLinkKey == link.SuggestionLinkKey);
            }

            if (dbLink == null)
            {
                if (link.ApplyToGroup)
                {
                    Santa_YearGroupUser? dbYearGroupUser = dbSantaUser.GiftingGroupYears
                        .FirstOrDefault(x => x.YearGroupUserKey == link.YearGroupUserKey);

                    if (dbYearGroupUser == null)
                    {
                        dbYearGroupUser = AddOrUpdateUserGroupYear(dbSantaUser, link.GiftingGroupKey, link.GiftingGroupName, true);
                    }

                    dbYearGroupUser.Included = true;

                    dbLink = new Santa_SuggestionLink
                    {
                        Suggestion = dbSuggestion,
                        YearGroupUserKey = link.YearGroupUserKey,
                        YearGroupUser = dbYearGroupUser
                    };

                    dbSuggestion.YearGroupUserLinks.Add(dbLink);
                }
            }
            else if (link.ApplyToGroup && dbLink.DateDeleted != null)
            {
                dbLink.DateDeleted = null;
            }
            else if (!link.ApplyToGroup)
            {
                dbLink.DateDeleted ??= DateTime.Now;
            }
        }
    }
}
