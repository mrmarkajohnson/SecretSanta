using Application.Areas.GiftingGroup.BaseModels;
using Application.Areas.GiftingGroup.Queries.Internal;
using Application.Areas.Messages.BaseModels;
using Global.Abstractions.Areas.GiftingGroup;
using Global.Extensions.Exceptions;
using static Global.Settings.GiftingGroupSettings;
using static Global.Settings.MessageSettings;

namespace Application.Areas.GiftingGroup.Commands;

public class SetupGiftingGroupYearCommand<TItem> : GiftingGroupYearBaseCommand<TItem> where TItem : IGiftingGroupYear
{
    private Global_User? _dbCurrentUser;

    public SetupGiftingGroupYearCommand(TItem item) : base(item)
    {
    }

    protected async override Task<ICommandResult<TItem>> HandlePostValidation()
    {
        if (Item.GiftingGroupKey == 0)
        {
            throw new NotFoundException($"Gifting Group '{Item.GiftingGroupName}'");
        }

        if (Item.Year == 0)
        {
            Item.Year = DateTime.Today.Year;
        }
        else if (Item.Year != DateTime.Today.Year)
        {
            throw new ArgumentException($"You cannot set up year {Item.Year} as it is not the current year."); // TODO: Allow years to continue into January, just in case
        }

        _dbCurrentUser = GetCurrentGlobalUser(g => g.SantaUser);
        if (_dbCurrentUser.SantaUser == null)
        {
            throw new AccessDeniedException();
        }

        Santa_GiftingGroupUser dbGiftingGroupLink = await Send(new GetGiftingGroupUserLinkQuery(Item.GiftingGroupKey, true));
        Santa_GiftingGroup dbGiftingGroup = dbGiftingGroupLink.GiftingGroup;
        Santa_GiftingGroupYear? dbGiftingGroupYear = dbGiftingGroup.Years.FirstOrDefault(x => x.Year == Item.Year);

        if (dbGiftingGroupYear == null)
        {
            dbGiftingGroupYear = CreateGiftingGroupYear(dbGiftingGroup);
        }

        foreach (IYearGroupUserBase member in Item.GroupMembers)
        {
            AddOrUpdateUserGroupYear(dbGiftingGroupYear, member.Included, member.SantaUserKey, member.UserDisplayName); // must be done before the next stage
        }

        if (!Validation.IsValid)
            return await Result();

        if (Item.CalculationOption == YearCalculationOption.None)
        {
            var participatingMembers = dbGiftingGroupYear.ParticipatingMembers();

            if (participatingMembers.Any(x => x.RecipientSantaUserKey != null))
            {
                if (participatingMembers.Any(x => x.RecipientSantaUserKey == null))
                    AddGeneralValidationError("Some participating group members have already been assigned a recipient," +
                        " but others haven't. Please recalculate or cancel recipients.");
                else if (dbGiftingGroupYear.Users.Any(x => !x.Included == true && x.RecipientSantaUserKey != null))
                    AddGeneralValidationError("Some non-participating group members have already been assigned a " +
                        "recipient. Please recalculate or cancel recipients.");

                if (!Validation.IsValid)
                    return await Result();
            }
        }
        else if (Item.CalculationOption == YearCalculationOption.Calculate)
        {
            var missingGroupMembers = dbGiftingGroupYear.ValidGroupMembers()
                .Where(x => dbGiftingGroup.UserLinks.Any(y => y.SantaUserKey == x.SantaUserKey) == false
                    || Item.GroupMembers.Any(y => y.SantaUserKey == x.SantaUserKey) == false);

            if (missingGroupMembers.Any())
            {
                foreach (var groupMember in missingGroupMembers)
                {
                    Item.GroupMembers.Add(Mapper.Map(groupMember, new YearGroupUserBase()));
                }

                AddGeneralValidationError("New members have been added to the group. Please try again.");
            }
            else if (dbGiftingGroupYear.Users
                .Where(x => dbGiftingGroupYear.ValidGroupMembers().Any(y => y.SantaUserKey == x.SantaUserKey))
                .Any(x => x.Included == null))
            {
                AddGeneralValidationError("Not all members have been set as participating or not.");
            }
            else
            {
                try
                {
                    await CalculateGiversAndReceivers(dbGiftingGroup, dbGiftingGroupYear);
                }
                catch (Exception exp)
                {
                    AddGeneralValidationError("Error calculating results for year " + Item.Year.ToString() + ": " + exp.Message);
                    return await Result();
                }
            }
        }
        else if (Item.CalculationOption == YearCalculationOption.Cancel)
        {
            foreach (var dbYearGroupUser in dbGiftingGroupYear.Users)
            {
                if (dbYearGroupUser.RecipientSantaUserKey != null)
                    SendRecipientMessage(dbYearGroupUser, true);

                dbYearGroupUser.RecipientSantaUserKey = null;
                dbYearGroupUser.RecipientSantaUser = null;
            }
        }

        if (!Validation.IsValid)
            return await Result();

        dbGiftingGroupYear.Limit = Item.Limit;
        dbGiftingGroupYear.CurrencyCode = Item.CurrencyCode ?? dbGiftingGroup.GetCurrencyCode();
        dbGiftingGroupYear.CurrencySymbol = Item.CurrencySymbol ?? dbGiftingGroup.GetCurrencySymbol();

        return await SaveAndReturnSuccess();
    }

    private async Task CalculateGiversAndReceivers(Santa_GiftingGroup dbGroup, Santa_GiftingGroupYear dbGiftingGroupYear)
    {
        int targetPreviousYears = 2;
        int actualPreviousYears = targetPreviousYears;
        List<GiverAndReceiverCombination> actualCombinations = await Send(new CalculateGiversAndReceiversQuery(dbGiftingGroupYear, ref actualPreviousYears));

        if (actualCombinations.Count > 0)
        {
            var participatingMembers = dbGiftingGroupYear.ParticipatingMembers();

            foreach (GiverAndReceiverCombination combi in actualCombinations)
            {
                Santa_YearGroupUser dbGiver = participatingMembers.First(x => x.SantaUserKey == combi.GiverSantaUserKey);
                int? existingRecipientSantaUserKey = dbGiver.RecipientSantaUserKey;

                dbGiver.RecipientSantaUserKey = combi.RecipientSantaUserKey;
                DbContext.ChangeTracker.DetectChanges();

                if (combi.RecipientSantaUserKey != existingRecipientSantaUserKey)
                    SendRecipientMessage(dbGiver, existingRecipientSantaUserKey == null ? false : null);
            }

            DbContext.ChangeTracker.DetectChanges();

            if (actualPreviousYears < targetPreviousYears)
            {
                string previousYears = "last year" + (actualPreviousYears > 1 ? " or the year before" : "");
                Item.PreviousYearsWarning = $"Some participating members have the same recipient as {previousYears}";
            }
        }
        else
        {
            AddGeneralValidationError("Error calculating results for year " + Item.Year.ToString()
                + ": " + "no possible combinations available");
        }
    }

    private void SendRecipientMessage(Santa_YearGroupUser dbGiver, bool? cancelled)
    {
        if (_dbCurrentUser?.SantaUser == null)
        {
            throw new AccessDeniedException();
        }

        string headerText = $"Your Secret Santa recipientfor group '{dbGiver.GiftingGroupYear.GiftingGroup.Name}' has been " + cancelled switch
        {
            true => "CANCELLED!",
            null => "CHANGED!",
            false => "chosen!"
        };

        string messageText = cancelled switch
        {
            true => $"All recipients for group '{dbGiver.GiftingGroupYear.GiftingGroup.Name}' this year have been cancelled and reset. " +
                "Look out for future messages telling you who your new recipient will be. " +
                "If you've already purchased a present, please contact a group administrator.",
            null => $"This year, you are NOW giving to {dbGiver.RecipientSantaUser.GlobalUser.FullName().ToUpper()}. "
                + $"All previous recipients for group '{dbGiver.GiftingGroupYear.GiftingGroup.Name}' this year have been cancelled and reset. " +
                "If you've already purchased a present, please contact a group administrator.",
            false => $"This year, you are giving to {dbGiver.RecipientSantaUser.GlobalUser.FullName().ToUpper()}."
        };

        var message = new SendSantaMessage
        {
            RecipientTypes = MessageRecipientType.GiftRecipient,
            HeaderText = headerText,
            MessageText = messageText,
            Important = cancelled != false,
            ShowAsFromSanta = true
        };

        SendMessage(message, _dbCurrentUser.SantaUser, dbGiver.SantaUser, dbGiver.GiftingGroupYear);
    }
}
