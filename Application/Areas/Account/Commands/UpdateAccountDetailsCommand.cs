using Application.Areas.Account.Actions;
using Global.Abstractions.Areas.Account;
using Microsoft.AspNetCore.Identity;

namespace Application.Areas.Account.Commands;

public sealed class UpdateAccountDetailsCommand<TItem> : IdentityBaseCommand<TItem> where TItem : IUpdateSantaUser
{
    public UpdateAccountDetailsCommand(TItem item, IUserStore<IdentityUser> userStore) : base(item, userStore)
    {
    }

    protected async override Task<ICommandResult<TItem>> HandlePostValidation()
    {
        EnsureSignedIn();

        string? globalUserId = GetCurrentUserId();
        if (globalUserId != null && globalUserId == Item.GlobalUserId)
        {
            Global_User? dbCurrentUser = GetGlobalUser(globalUserId);

            if (dbCurrentUser != null)
            {
                bool passwordCorrect = await CheckPasswordAndHandleFailure(Item, dbCurrentUser);
                if (!passwordCorrect || !Validation.IsValid)
                {
                    return await Result();
                }
                else
                {
                    string? originalUserName = Item.UserName; // get this before it is hashed
                    string? originalEmail = Item.Email; // ditto;
                    await Send(new HashUserIdentificationAction(Item)); // now hash it

                    await HandleUserName(dbCurrentUser, originalUserName, originalEmail);
                    await HandleEmailAddress(dbCurrentUser, originalUserName, originalEmail);

                    if (Validation.IsValid)
                    {
                        dbCurrentUser.Forename = Item.Forename.Tidy();
                        dbCurrentUser.Surname = Item.Surname.Tidy();
                        dbCurrentUser.Email = Item.Email.NullIfEmpty().Tidy(false);
                        dbCurrentUser.UserName = Item.UserName.NullIfEmpty().Tidy(false);

                        SetOtherNames(dbCurrentUser);

                        return await SaveAndReturnSuccess();
                    }
                }
            }
            else
            {
                AddUserNotFoundError();
            }
        }
        else
        {
            AddUserNotFoundError();
        }

        return await Result();
    }

    private async Task HandleUserName(Global_User dbCurrentUser, string? originalUserName, string? originalEmail)
    {
        if (Item.UserName != dbCurrentUser.UserName) // hashed version in both cases
        {
            try
            {
                await SetUserName(dbCurrentUser);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                message = ReplaceHashedDetails(message, originalUserName, originalEmail);
                AddValidationError(nameof(Item.UserName), message);
            }
        }
    }

    private async Task HandleEmailAddress(Global_User dbCurrentUser, string? originalUserName, string? originalEmail)
    {
        if (Item.Email != dbCurrentUser.Email) // hashed version in both cases
        {
            try
            {
                await StoreEmailAddress(dbCurrentUser, originalEmail);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                message = ReplaceHashedDetails(message, originalUserName, originalEmail);
                AddValidationError(nameof(Item.Email), message);
            }
        }
    }
}
