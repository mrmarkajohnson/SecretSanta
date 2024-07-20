using Application.Shared.Identity;
using Data.Entities.Shared;
using Global.Abstractions.Global.Account;
using Global.Abstractions.Santa.Areas.Account;
using Microsoft.AspNetCore.Identity;
using SecretSanta.Data;

namespace Application.Santa.Global;

public abstract class BaseRequest
{
    protected ApplicationDbContext ModelContext { get; set; }

    protected BaseRequest()
    {
        ModelContext = new ApplicationDbContext();
    }

    protected async Task<TItem> Send<TItem>(BaseQuery<TItem> query)
    {
        return await query.Handle();
    }

    protected async Task<bool> Send<TItem>(BaseAction<TItem> action)
    {
        return await action.Handle();
    }

    protected Global_User? GetGlobalUser(IIdentityUser user)
    {
        return GetGlobalUser(user.Id);
    }

    protected Global_User? GetGlobalUser(string userId)
    {
        return ModelContext.Global_Users.FirstOrDefault(x => x.Id == userId);
    }

    protected async Task<bool> AccessFailed(UserManager<IdentityUser> userManager, IIdentityUser user)
    {
        if (user != null)
        {
            var dbUser = await userManager.FindByIdAsync(user.Id); // always get the user again, to avoid double tracking errors
            if (dbUser != null)
            {
                await userManager.AccessFailedAsync(dbUser);
                return dbUser.LockoutEnd != null && dbUser.LockoutEnd > DateTime.Now;
            }
        }

        return false;        
    }
}
