using Global.Abstractions.Santa.Areas.Account;
using Microsoft.AspNetCore.Identity;

namespace Application.Santa.Areas.Account.Commands;

public abstract class IdentityBaseCommand<TItem> : UserBaseCommand<TItem> where TItem : ISantaUser
{
    private protected IUserStore<IdentityUser> UserStore { get; set; }

    public IdentityBaseCommand(TItem item, IUserStore<IdentityUser> userStore) : base(item)
    {
        UserStore = userStore;
    }

    private protected IUserEmailStore<IdentityUser> GetEmailStore()
    {
        if (!UserManager.SupportsUserEmail)
        {
            throw new NotSupportedException("The default UI requires a user store with email support.");
        }

        return (IUserEmailStore<IdentityUser>)UserStore;
    }

    private protected async Task SetUserName(Global_User dbGlobalUser) // use this approach so it is thoroughly checked
    {
        await UserStore.SetUserNameAsync(dbGlobalUser, Item.UserName, CancellationToken.None);
    }

    private protected async Task StoreEmailAddress(Global_User dbGlobalUser) // use this approach so it is thoroughly checked
    {
        if (!string.IsNullOrWhiteSpace(Item.Email))
        {
            try
            {
                await GetEmailStore().SetEmailAsync(dbGlobalUser, Item.Email, CancellationToken.None);
            }
            catch
            {
                try
                {
                    await UserManager.SetEmailAsync(dbGlobalUser, Item.Email);
                }
                catch
                {
                    string token = await UserManager.GenerateChangeEmailTokenAsync(dbGlobalUser, Item.Email);
                    await UserManager.ChangeEmailAsync(dbGlobalUser, Item.Email, token);
                }
            }
        }
        else
        {
            dbGlobalUser.Email = null;
        }
    }

    private protected string ReplaceHashedDetails(string message, string? originalUserName, string? originalEmail)
    {
        if (!string.IsNullOrWhiteSpace(Item.UserName))
        {
            message = message.Replace(Item.UserName, originalUserName);
        }

        if (!string.IsNullOrWhiteSpace(Item.Email))
        {
            message = message.Replace(Item.Email, originalEmail);
        }

        return message;
    }
}
