using Application.Areas.GiftingGroup.Commands;
using Application.Areas.Messages.BaseModels;
using Global.Abstractions.Areas.Suggestions;
using Global.Extensions.Exceptions;
using static Global.Settings.MessageSettings;

namespace Application.Areas.Suggestions.Commands;

public class SaveSuggestionCommand<TItem> : GiftingGroupYearBaseCommand<TItem> where TItem : class, IManageSuggestion
{
    private string _yearGroupUrl;

    public SaveSuggestionCommand(TItem item, string yearGroupUrl) : base(item)
    {
        _yearGroupUrl = yearGroupUrl;
    }

    protected async override Task<ICommandResult<TItem>> HandlePostValidation()
    {
        Santa_User dbCurrentSantaUser = GetCurrentSantaUser(s => s.GiftingGroupLinks, s => s.Suggestions);
        Santa_Suggestion? dbSuggestion = null;

        if (Item.SuggestionKey > 0)
        {
            dbSuggestion = dbCurrentSantaUser.Suggestions.FirstOrDefault(x => x.SuggestionKey == Item.SuggestionKey);
        }
        else
        {
            dbSuggestion = new Santa_Suggestion
            {
                SantaUserKey = dbCurrentSantaUser.SantaUserKey,
                SantaUser = dbCurrentSantaUser
            };

            dbCurrentSantaUser.Suggestions.Add(dbSuggestion);
        }

        if (dbSuggestion == null)
        {
            throw new NotFoundException("Suggestion");
        }

        dbSuggestion.SuggestionText = Item.SuggestionText;
        dbSuggestion.OtherNotes = Item.OtherNotes;
        dbSuggestion.Priority = Item.Priority;

        HandleGroupLinks(dbCurrentSantaUser, dbSuggestion);

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
                    MessageGifter(dbSantaUser, dbYearGroupUser);
                }
            }
            else if (link.ApplyToGroup && dbLink.DateDeleted != null)
            {
                dbLink.DateDeleted = null;
                MessageGifter(dbSantaUser, dbLink.YearGroupUser);
            }
            else if (!link.ApplyToGroup)
            {
                dbLink.DateDeleted ??= DateTime.Now;
                MessageGifter(dbSantaUser, dbLink.YearGroupUser, true);
            }
        }
    }

    private void MessageGifter(Santa_User dbSantaUser, Santa_YearGroupUser dbYearGroupUser, bool deleted = false)
    {
        var dbYear = dbYearGroupUser.GiftingGroupYear;
        var dbGifter = dbYear.Users.FirstOrDefault(x => x.RecipientSantaUserKey == dbSantaUser.SantaUserKey);
        string yearGroupUrl = _yearGroupUrl.Replace("giftingGroupKey=0", $"giftingGroupKey={dbYear.GiftingGroupKey}");

        if (dbGifter != null)
        {
            string text = (deleted ? "A new suggestion has been added for" : "A suggestion has been removed from") +
                $" the '{dbYear.GiftingGroup.Name}' group. Please review " +
                (deleted ? "this suggestion" : "your recipient's suggestions") +
                $" at the {MessageLink(yearGroupUrl, "Gifting Group Year", false)} page.";

            var message = new SendSantaMessage
            {
                RecipientType = MessageRecipientType.Gifter,
                HeaderText = "Your gift recipient has " + (deleted ? "removed a " : "added a new") + " suggestion",
                MessageText = text,
                Important = true,
                CanReply = false,
                ShowAsFromSanta = true
            };

            SendMessage(message, dbSantaUser, dbGifter.SantaUser, dbYearGroupUser.GiftingGroupYear);
        }
    }
}
