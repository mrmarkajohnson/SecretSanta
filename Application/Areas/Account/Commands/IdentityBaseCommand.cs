using Global.Abstractions.Areas.Account;
using Microsoft.AspNetCore.Identity;
using static Global.Settings.IdentitySettings;

namespace Application.Areas.Account.Commands;

public abstract class IdentityBaseCommand<TItem> : UserBaseCommand<TItem> where TItem : ISantaUser
{
    private protected IUserStore<IdentityUser> UserStore { get; set; }

    public IdentityBaseCommand(TItem item, IUserStore<IdentityUser> userStore) : base(item)
    {
        UserStore = userStore;
    }

    private protected async Task SetUserName(Global_User dbGlobalUser) // use this approach so it is thoroughly checked
    {
        await UserStore.SetUserNameAsync(dbGlobalUser, Item.UserName, CancellationToken.None);
    }

    private protected async Task StoreEmailAddress(Global_User dbGlobalUser, string? unhashedEmail) // use this approach so it is thoroughly checked
    {
        await StoreEmailAddress(dbGlobalUser, Item.Email, UserStore, unhashedEmail);
    }

    private protected void SetOtherNames(Global_User dbCurrentUser)
    {
        string? preferredFirstName = Item.PreferredNameType switch
        {
            PreferredNameOption.Forename => null,
            PreferredNameOption.MiddleName => Item.MiddleNames,
            _ => Item.PreferredFirstName.NullIfEmpty()

        };

        dbCurrentUser.MiddleNames = Item.MiddleNames.NullIfEmpty();
        dbCurrentUser.PreferredNameType = Item.PreferredNameType;
        dbCurrentUser.PreferredFirstName = preferredFirstName;        
    }
}
