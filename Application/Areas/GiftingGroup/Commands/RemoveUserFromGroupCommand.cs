using Application.Areas.GiftingGroup.BaseModels;
using Application.Areas.GiftingGroup.Queries.Internal;

namespace Application.Areas.GiftingGroup.Commands;

public class RemoveUserFromGroupCommand : BaseCommand<ChangeGroupMemberStatus>
{
    public RemoveUserFromGroupCommand(ChangeGroupMemberStatus item, string participateUrl) : base(item)
    {
        _participateUrl = participateUrl;
    }

    private string _participateUrl;

    protected async override Task<ICommandResult<ChangeGroupMemberStatus>> HandlePostValidation()
    {
        Santa_GiftingGroupUser dbGiftingGroupLink = await Send(new GetGiftingGroupUserLinkQuery(Item.GiftingGroupKey, true));

        if (dbGiftingGroupLink == null)
        {
            AddGeneralValidationError("Group not found.");
            return await Result();
        }

        if (!dbGiftingGroupLink.GroupAdmin)
        {
            AddGeneralValidationError("Only group administrators can remove another group member.");
            return await Result();
        }

        Santa_GiftingGroup dbGiftingGroup = dbGiftingGroupLink.GiftingGroup;

        Santa_GiftingGroupUser? dbMemberLink = dbGiftingGroup.Members
            .Where(x => x.DateDeleted == null && x.DateArchived == null)
            .FirstOrDefault(x => x.SantaUserKey == Item.SantaUserKey);

        if (dbMemberLink != null && dbMemberLink.DateArchived == null)
        {
            Santa_YearGroupUser? dbCalculateYearLink = dbGiftingGroup.Years
                .Where(x => x.CalendarYear == DateTime.Today.Year)
                .SelectMany(x => x.Users)
                .Where(y => y.SantaUserKey == Item.SantaUserKey)
                .Where(y => y.RecipientSantaUserKey > 0)
                .FirstOrDefault();

            Global_User dbGlobalUser = dbMemberLink.SantaUser.GlobalUser;

            if (dbCalculateYearLink != null)
            {
                AddGeneralValidationError($"{dbGlobalUser.DisplayName(false)} is already participating in this year's 'draw'. " +
                    $"Please first go to " +
                    $"{DisplayLink(_participateUrl, $"Set Up Group '{dbGiftingGroup.Name}' for {DateTime.Today.Year}", true)} " +
                    $"to remove {dbGlobalUser.Gender.Indirect()} from this year, and recalculate givers and receivers.");
            }
            else
            {                
                dbMemberLink.DateArchived = DateTime.Now;
                return await SaveAndReturnSuccess($"{dbGlobalUser.DisplayName(false)} has successfully been removed from group '{dbGiftingGroup.Name}'.");
            }
        }
        else
        {
            AddGeneralValidationError("User not found, or not a member of the group.");
        }

        return await Result();
    }
}
