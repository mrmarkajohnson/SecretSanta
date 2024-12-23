using Application.Santa.Areas.GiftingGroup.BaseModels;
using Application.Santa.Areas.GiftingGroup.Queries.Internal;
using Global.Abstractions.Santa.Areas.GiftingGroup;
using Global.Extensions.Exceptions;
using static Global.Settings.GiftingGroupSettings;
using static Global.Settings.MessageSettings;

namespace Application.Santa.Areas.GiftingGroup.Commands;

public class SetupGiftingGroupYearCommand<TItem> : BaseCommand<TItem> where TItem : IGiftingGroupYear
{
    private Global_User? _dbCurrentUser;

    public SetupGiftingGroupYearCommand(TItem item) : base(item)
    {
    }

    public class CombinationNumber
    {
        public int GiverId { get; init; }
        public int PossibleCombinationCount { get; set; }
    }

    protected async override Task<ICommandResult<TItem>> HandlePostValidation()
    {
        if (Item.GiftingGroupId == 0)
        {
            throw new NotFoundException("Gifting Group");
        }

        if (Item.Year == 0)
        {
            Item.Year = DateTime.Today.Year;
        }
        else if (Item.Year != DateTime.Today.Year)
        {
            throw new ArgumentException($"You cannot set up year {Item.Year} as it is not the current year."); // TODO: Allow years to continue into January, just in case
        }

        _dbCurrentUser = GetCurrentGlobalUser();

        Santa_GiftingGroupUser dbGiftingGroupLink = await Send(new GetGiftingGroupUserLinkQuery(Item.GiftingGroupId, true));
        Santa_GiftingGroup dbGroup = dbGiftingGroupLink.GiftingGroup;
        Santa_GiftingGroupYear? dbGiftingGroupYear = dbGroup.Years.FirstOrDefault(x => x.Year == Item.Year);

        if (dbGiftingGroupYear == null)
        {
            dbGiftingGroupYear = new Santa_GiftingGroupYear
            {
                GiftingGroup = dbGroup,
                Year = Item.Year
            };

            dbGroup.Years.Add(dbGiftingGroupYear);
            DbContext.ChangeTracker.DetectChanges();
        }

        foreach (IYearGroupUserBase member in Item.GroupMembers)
        {
            Santa_YearGroupUser? dbYearUser = dbGiftingGroupYear.Users.FirstOrDefault(x => x.SantaUserId == member.SantaUserId);

            if (dbYearUser == null)
            {
                var dbSantaUser = DbContext.Santa_Users.FirstOrDefault(x => x.Id == member.SantaUserId);

                if (dbSantaUser == null)
                {
                    AddGeneralValidationError($"User {member.UserDisplayName} could not be found.");
                }
                else
                {
                    dbYearUser = new Santa_YearGroupUser
                    {
                        YearId = dbGiftingGroupYear.Id,
                        Year = dbGiftingGroupYear,
                        SantaUserId = member.SantaUserId,
                        SantaUser = dbSantaUser,
                        Included = member.Included
                    };

                    dbGiftingGroupYear.Users.Add(dbYearUser);
                }
            }
            else
            {
                dbYearUser.Included = member.Included; // must be set before the next stage
            }
        }

        if (!Validation.IsValid)
            return await Result();

        if (Item.CalculationOption == YearCalculationOption.None)
        {
            var participatingMembers = dbGiftingGroupYear.ParticipatingMembers();

            if (participatingMembers.Any(x => x.GivingToUserId != null))
            {
                if (participatingMembers.Any(x => x.GivingToUserId != null))
                    AddGeneralValidationError("Some participating group members have already been assigned a recipient," +
                        " but others haven't. Please recalculate or cancel recipients.");
                else if (dbGiftingGroupYear.Users.Any(x => !x.Included == true && x.GivingToUserId != null))
                    AddGeneralValidationError("Some non-participating group members have already been assigned a " +
                        "recipient. Please recalculate or cancel recipients.");

                if (!Validation.IsValid)
                    return await Result();
            }
        }
        else if (Item.CalculationOption == YearCalculationOption.Calculate)
        {
            var missingGroupMembers = dbGiftingGroupYear.ValidGroupMembers()
                .Where(x => dbGroup.UserLinks.Any(y => y.SantaUserId == x.SantaUserId) == false
                    || Item.GroupMembers.Any(y => y.SantaUserId == x.SantaUserId == false));

            if (missingGroupMembers.Any())
            {
                foreach (var groupMember in missingGroupMembers)
                {
                    Item.GroupMembers.Add(Mapper.Map(groupMember, new YearGroupUserBase()));
                }

                AddGeneralValidationError("New members have been added to the group. Please try again.");
            }
            else if (dbGiftingGroupYear.Users
                .Where(x => dbGiftingGroupYear.ValidGroupMembers().Any(y => y.SantaUserId == x.SantaUserId))
                .Any(x => x.Included == null))
            {
                AddGeneralValidationError("Not all members have been set as participating or not.");
            }
            else
            {
                try
                {
                    await CalculateGiversAndReceivers(dbGroup, dbGiftingGroupYear);
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
            foreach (var dbUser in dbGiftingGroupYear.Users)
            {
                if (dbUser.GivingToUserId != null)
                    SendRecipientMessage(dbUser, true);

                dbUser.GivingToUserId = null;
                dbUser.GivingToUser = null;
            }
        }

        if (!Validation.IsValid)
            return await Result();

        dbGiftingGroupYear.Limit = Item.Limit;

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
                Santa_YearGroupUser dbGiver = participatingMembers.First(x => x.SantaUserId == combi.GiverId);
                int? existingRecipientId = dbGiver.GivingToUserId;

                dbGiver.GivingToUserId = combi.RecipientId;
                DbContext.ChangeTracker.DetectChanges();

                if (combi.RecipientId != existingRecipientId)
                    SendRecipientMessage(dbGiver, existingRecipientId == null ? false : null);
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
        string headerText = $"Your Secret Santa recipientfor group '{dbGiver.Year.GiftingGroup.Name}' has been " + cancelled switch
        {
            true => "CANCELLED!",
            null => "CHANGED!",
            false => "chosen!"
        }; 
        
        string messageText = cancelled switch
        {
            true => $"All recipients for group '{dbGiver.Year.GiftingGroup.Name}' this year have been cancelled and reset. " +
                "Look out for future messages telling you who your new recipient will be. " +
                "If you've already purchased a present, please contact a group administrator.",
            null => $"This year, you are NOW giving to {dbGiver.GivingToUser.GlobalUser.FullName().ToUpper()}. "
                + $"All previous recipients for group '{dbGiver.Year.GiftingGroup.Name}' this year have been cancelled and reset. " +
                "If you've already purchased a present, please contact a group administrator.",
            false => $"This year, you are giving to {dbGiver.GivingToUser.GlobalUser.FullName().ToUpper()}."
        };

        var dbMessage = new Santa_Message
        {
            Sender = _dbCurrentUser.SantaUser,
            ShowAsFromSanta = true,
            Important = cancelled != false,
            HeaderText = headerText,
            MessageText = messageText,
            RecipientTypes = MessageRecipientType.GiftRecipient
        };

        dbMessage.Recipients.Add(new Santa_MessageRecipient
        {
            Message = dbMessage,
            Recipient = dbGiver.SantaUser
        });
    }
}
