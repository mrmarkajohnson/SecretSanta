using Application.Areas.GiftingGroup.Actions;
using Application.Shared.Requests;
using Global.Abstractions.Areas.GiftingGroup;
using Global.Extensions.Exceptions;

namespace Application.Areas.GiftingGroup.Commands;

public class JoinGiftingGroupCommand<TItem> : BaseCommand<TItem> where TItem : IJoinGiftingGroup
{
    public JoinGiftingGroupCommand(TItem item) : base(item)
    {
    }

    protected async override Task<ICommandResult<TItem>> HandlePostValidation()
    {
        EnsureSignedIn();

        Santa_GiftingGroup? dbGiftingGroup = null;

        bool found = await Send(new AddGroupDetailsForJoinerAction(Item));

        if (!found)
        {
            AddGeneralValidationError("No matching group found.");
            return await Result();
        }

        if (Item.AlreadyMember)
        {
            AddGeneralValidationError("You are already a member of this group.");
            return await Result();
        }

        if (Item.Blocked)
        {
            AddGeneralValidationError("You are blocked from applying to join this group. Please stop sending applications.");
            return await Result();
        }

        if (Item.ApplicationPending)
        {
            AddGeneralValidationError("You have already requested to join this group. " +
                "A group administrator will review your request soon.");
            return await Result();
        }

        if (Validation.IsValid)
        {
            dbGiftingGroup = DbContext.Santa_GiftingGroups.Where(x => x.GiftingGroupKey == Item.GiftingGroupKey).FirstOrDefault();

            if (dbGiftingGroup == null) // just in case
            {
                AddGeneralValidationError("No matching group found.");
                return await Result();
            }

            Santa_User dbCurrentSantaUser = GetCurrentSantaUser(s => s.GiftingGroupLinks);
            if (dbCurrentSantaUser == null)
            {
                throw new AccessDeniedException();
            }

            var dbApplication = new Santa_GiftingGroupApplication
            {
                SantaUser = dbCurrentSantaUser,
                GiftingGroup = dbGiftingGroup,
                Message = Item.Message
            };

            DbContext.Add(dbApplication);

            return await SaveAndReturnSuccess();
        }

        return await Result();
    }
}
