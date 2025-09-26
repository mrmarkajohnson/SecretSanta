using Application.Areas.GiftingGroup.BaseModels;
using Application.Areas.GiftingGroup.Queries.Internal;

namespace Application.Areas.GiftingGroup.Commands;

public class RemoveUserFromGroupCommand : BaseCommand<ChangeGroupMemberStatus>
{
    public RemoveUserFromGroupCommand(ChangeGroupMemberStatus item) : base(item)
    {
    }

    protected async override Task<ICommandResult<ChangeGroupMemberStatus>> HandlePostValidation()
    {
        Santa_GiftingGroupUser dbGiftingGroupLink = await Send(new GetGiftingGroupUserLinkQuery(Item.GiftingGroupKey, true));

        if (dbGiftingGroupLink == null || !dbGiftingGroupLink.GroupAdmin)
            return await Result();

        var dbMemberLink = dbGiftingGroupLink.GiftingGroup.Members
            .Where(x => x.DateDeleted == null && x.DateArchived == null)
            .FirstOrDefault(x => x.SantaUserKey == Item.SantaUserKey);

        if (dbMemberLink != null && dbMemberLink.DateArchived == null)
        {
            dbMemberLink.DateArchived = DateTime.Now;
            DbContext.SaveChanges();
        }

        return await Result();
    }
}
