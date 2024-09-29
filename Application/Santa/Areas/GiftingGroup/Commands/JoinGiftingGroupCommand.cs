using Global.Abstractions.Santa.Areas.GiftingGroup;
using Global.Extensions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.Santa.Areas.GiftingGroup.Commands;

public class JoinGiftingGroupCommand<T> : BaseCommand<T> where T : IJoinGiftingGroup
{
    public JoinGiftingGroupCommand(T item) : base(item)
    {
    }

    protected async override Task<ICommandResult<T>> HandlePostValidation()
    {
        EnsureSignedIn();

        Santa_GiftingGroup? dbGiftingGroup = null;

        if (Item.GroupId > 0)
        {
            dbGiftingGroup = ModelContext.Santa_GiftingGroups.Where(x => x.Id == Item.GroupId).FirstOrDefault();

            if (dbGiftingGroup?.Name != Item.Name || dbGiftingGroup?.JoinerToken != Item.JoinerToken)
            {
                dbGiftingGroup = null;
            }
        }

        if (dbGiftingGroup == null)
        {
            dbGiftingGroup = ModelContext.Santa_GiftingGroups
                .Where(x => x.Name == Item.Name && x.JoinerToken == Item.JoinerToken)
                .FirstOrDefault();
        }

        if (dbGiftingGroup == null)
        {
            AddValidationError(string.Empty, "No matching group found.");
            return await Result();
        }

        Global_User? dbGlobalUser = GetCurrentGlobalUser(g => g.SantaUser, g => g.SantaUser.GiftingGroupLinks);
        if (dbGlobalUser == null || dbGlobalUser.SantaUser == null)
        {
            throw new AccessDeniedException();
        }

        var existingGroupLinks = dbGlobalUser.SantaUser.GiftingGroupLinks
            .Where(x => x.DateDeleted == null && x.GiftingGroup.DateDeleted == null)
            .Where(x => x.GiftingGroupId == dbGiftingGroup.Id);

        if (existingGroupLinks.Any())
        {
            AddValidationError(string.Empty, "You are already a member of this group.");
            return await Result();
        }

        var previousApplications = dbGlobalUser.SantaUser.GiftingGroupApplications
            .Where(x => x.DateDeleted == null && x.GiftingGroup.DateDeleted == null)
            .Where(x => x.GiftingGroupId == dbGiftingGroup.Id)
            .ToList();

        var blockedApplications = previousApplications
            .Where(x => x.Accepted == null);

        if (blockedApplications.Any())
        {
            AddValidationError(string.Empty, "You are blocked from applying to join this group. Please stop sending applications.");
            Item.Blocked = true;
            return await Result();
        }

        var pendingApplications = previousApplications
            .Where(x => x.Accepted == null);

        if (pendingApplications.Any())
        {
            AddValidationError(string.Empty, "You have already requested to join this group. " +
                "A group administrator will review your request soon.");
            return await Result();
        }

        if (Validation.IsValid)
        {
            var dbApplication = new Santa_GiftingGroupApplication
            { 
                User = dbGlobalUser.SantaUser,
                GiftingGroup = dbGiftingGroup,
                Message = Item.Message
            };

            ModelContext.Add(dbApplication);

            await ModelContext.SaveChangesAsync();
            Success = true;
        }

        return await Result();
    }
}
