using Global.Abstractions.Santa.Areas.GiftingGroup;
using Global.Extensions.Exceptions;

namespace Application.Santa.Areas.GiftingGroup.Commands;

public class ReviewJoinerApplicationCommand<TItem> : BaseCommand<TItem> where TItem : IReviewApplication
{
    public ReviewJoinerApplicationCommand(TItem item) : base(item)
    {
    }

    protected async override Task<ICommandResult<TItem>> HandlePostValidation()
    {
        Global_User? dbGlobalUser = GetCurrentGlobalUser(g => g.SantaUser, g => g.SantaUser.GiftingGroupLinks);
        if (dbGlobalUser == null || dbGlobalUser.SantaUser == null)
        {
            throw new AccessDeniedException();
        }

        var dbApplication = dbGlobalUser.SantaUser?.GiftingGroupLinks
            .Where(x => x.DateDeleted == null && x.GiftingGroup != null && x.GiftingGroup.DateDeleted == null && x.GroupAdmin)
            .Select(x => x.GiftingGroup)
            .SelectMany(x => x.MemberApplications)
            .Where(x => x.Id == Item.ApplicationId)
            .FirstOrDefault();

        if (dbApplication == null)
        {
            dbApplication = DbContext.Santa_GiftingGroupApplications.FirstOrDefault(x => x.Id == Item.ApplicationId);

            if (dbApplication != null && dbApplication.GiftingGroup.DateDeleted == null)
            {
                var dbLinks = dbGlobalUser.SantaUser?.GiftingGroupLinks
                    .Where(x => x.GiftingGroupId == dbApplication.GiftingGroupId && x.GroupAdmin)
                    .ToList();

                if (!dbLinks?.Any() == true)
                {
                    throw new AccessDeniedException();
                }
            }

            throw new NotFoundException("application");
        }

        if (Validation.IsValid)
        {
            dbApplication.Accepted = Item.Accepted;
            dbApplication.RejectionMessage = Item.Accepted ? null : Item.RejectionMessage;
            dbApplication.Blocked = Item.Accepted ? false : Item.Blocked;
            dbApplication.ResponseByUserId = dbGlobalUser.SantaUser?.Id;

            return await SaveAndReturnSuccess();
        }

        return await Result();
    }
}
