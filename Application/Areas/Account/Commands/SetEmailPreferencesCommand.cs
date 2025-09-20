using Microsoft.AspNetCore.Identity;

namespace Application.Areas.Account.Commands;

public class SetEmailPreferencesCommand<TItem> : UserBaseCommand<TItem> where TItem : IUserEmailDetails
{
    private IUserStore<IdentityUser> _userStore;

    public SetEmailPreferencesCommand(TItem item, IUserStore<IdentityUser> userStore) : base(item)
    {
        _userStore = userStore;
    }

    protected async override Task<ICommandResult<TItem>> HandlePostValidation()
    {
        Santa_User dbCurrentSantaUser = GetCurrentSantaUser();

        dbCurrentSantaUser.ReceiveEmails = Item.ReceiveEmails;
        dbCurrentSantaUser.DetailedEmails = Item.DetailedEmails;

        if (Validation.IsValid)
        {
            string? originalEmail = Item.Email = TidyEmail(Item.Email); // ditto;

            if (Item.Email.IsNotEmpty())
            {
                Item.Email = EncryptionHelper.EncryptEmail(Item.Email);
            }

            await HandleEmailAddress(dbCurrentSantaUser.GlobalUser, originalEmail);
        }

        return await SaveAndReturnSuccess();
    }

    private async Task HandleEmailAddress(Global_User dbCurrentUser, string? originalEmail)
    {
        if (Item.Email != dbCurrentUser.Email)  // hashed version in both cases
        {
            try
            {
                await StoreEmailAddress(dbCurrentUser, Item.Email, _userStore, originalEmail);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                message = ReplaceHashedDetails(message, dbCurrentUser.UserName, originalEmail);
                AddValidationError(nameof(Item.Email), message);
            }
        }
    }
}
