using Application.Santa.Areas.GiftingGroup.Actions;
using Global.Abstractions.Santa.Areas.GiftingGroup;
using Global.Extensions.Exceptions;

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

        bool found = await Send(new AddGroupDetailsForJoinerAction(Item));        

        if (!found)
        {
            AddValidationError(string.Empty, "No matching group found.");
            return await Result();
        }

        if (Item.AlreadyMember)
        {
            AddValidationError(string.Empty, "You are already a member of this group.");
            return await Result();
        }

        if (Item.Blocked)
        {
            AddValidationError(string.Empty, "You are blocked from applying to join this group. Please stop sending applications.");
            return await Result();
        }

        if (Item.ApplicationPending)
        {
            AddValidationError(string.Empty, "You have already requested to join this group. " +
                "A group administrator will review your request soon.");
            return await Result();
        }

        if (Validation.IsValid)
        {
            dbGiftingGroup = ModelContext.Santa_GiftingGroups.Where(x => x.Id == Item.GroupId).FirstOrDefault();

            if (dbGiftingGroup == null) // just in case
            {
                AddValidationError(string.Empty, "No matching group found.");
                return await Result();
            }

            Global_User? dbGlobalUser = GetCurrentGlobalUser(g => g.SantaUser, g => g.SantaUser.GiftingGroupLinks);
            
            if (dbGlobalUser == null || dbGlobalUser.SantaUser == null) // just in case
            {
                throw new AccessDeniedException();
            }

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
