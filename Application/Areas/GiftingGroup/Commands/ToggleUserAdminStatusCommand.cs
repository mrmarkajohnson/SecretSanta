using Application.Areas.GiftingGroup.BaseModels;
using Application.Areas.GiftingGroup.Queries.Internal;

namespace Application.Areas.GiftingGroup.Commands;

public class ToggleUserAdminStatusCommand : BaseCommand<ChangeGroupMemberStatus>
{
    public ToggleUserAdminStatusCommand(ChangeGroupMemberStatus item) : base(item)
    {
    }

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
            AddGeneralValidationError("Only group administrators can change the status of another group member.");
            return await Result();
        }

        var dbMemberLink = dbGiftingGroupLink.GiftingGroup.Members
            .Where(x => x.DateDeleted == null && x.DateArchived == null)
            .FirstOrDefault(x => x.SantaUserKey == Item.SantaUserKey);

        if (dbMemberLink != null)
        {
            dbMemberLink.GroupAdmin = !dbMemberLink.GroupAdmin;
            string changed = dbMemberLink.GroupAdmin ? "set" : "removed";
            return await SaveAndReturnSuccess($"Administrator status {changed} successfully.");
        }
        else
        {
            AddGeneralValidationError("Group member not found.");
            return await Result();
        }
    }
}
