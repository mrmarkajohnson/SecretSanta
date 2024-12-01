using Application.Santa.Areas.Account.BaseModels;
using Application.Santa.Areas.GiftingGroup.BaseModels;
using Application.Santa.Areas.GiftingGroup.Queries.Internal;
using Global.Abstractions.Santa.Areas.GiftingGroup;
using Global.Extensions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Global.Settings.GiftingGroupSettings;

namespace Application.Santa.Areas.GiftingGroup.Commands;

public class SetupGiftingGroupYearCommand<TItem> : BaseCommand<TItem> where TItem : IGiftingGroupYear
{
    public SetupGiftingGroupYearCommand(TItem item) : base(item)
    {
    }

    protected async override Task<ICommandResult<TItem>> HandlePostValidation()
    {
        if (Item.GiftingGroupId == 0)
        {
            throw new NotFoundException("Gifting Group");
        }

        if (Item.Year == 0)
            Item.Year = DateTime.Today.Year;
        else if (Item.Year != DateTime.Today.Year)
        {
            throw new ArgumentException($"You cannot set up year {Item.Year} as it is not the current year."); // TODO: Allow years to continue into January, just in case
        }

        Santa_GiftingGroupUser dbGiftingGroupLink = await Send(new GetGiftingGroupUserLinkQuery(Item.GiftingGroupId, true));
        Santa_GiftingGroup dbGroup = dbGiftingGroupLink.GiftingGroup;
        Santa_GiftingGroupYear? dbGiftingGroupYear = dbGroup.Years.FirstOrDefault(x => x.Year ==  Item.Year);

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
                }
            }
        }

        if (!Validation.IsValid)
            return await Result();

        if (Item.CalculationOption == YearCalculationOption.Calculate)
        {
            DateTime firstDayOfNextYear = new DateTime(Item.Year + 1, 1, 1);

            var validGroupMembers = dbGroup.UserLinks
                .Where(x => x.DateDeleted == null && (x.DateArchived == null || x.DateArchived < firstDayOfNextYear));

            var missingGroupMembers = validGroupMembers
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
            else
            {
                // TODO: Calculate
                // TODO: Create messages to users telling them their recipient
            }
        }
        else if (Item.CalculationOption == YearCalculationOption.Cancel)
        {
            foreach (var user in dbGiftingGroupYear.Users)
            {
                user.GivingToUserId = null;
                user.GivingToUser = null;
            }

            // TODO: Create messages to users telling them the setup has been cancelled
        }

        if (!Validation.IsValid)
            return await Result();

        dbGiftingGroupYear.Limit = Item.Limit;

        return await SaveAndReturnSuccess();
    }
}
