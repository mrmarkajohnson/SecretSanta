using Global.Abstractions.Santa.Areas.GiftingGroup;
using Global.Extensions.Exceptions;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Application.Santa.Areas.GiftingGroup.Commands;

public class SaveGiftingGroupCommand<T> : BaseCommand<T> where T : IGiftingGroup
{
    private readonly ClaimsPrincipal _user;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public SaveGiftingGroupCommand(T item,
        ClaimsPrincipal user,
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager) : base(item)
    {
        _user = user;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    protected async override Task<ICommandResult<T>> HandlePostValidation()
    {
        EnsureSignedIn(_user, _signInManager);

        Santa_GiftingGroup? dbItem = null;

        if (Item.Id > 0)
        {
            Global_User? dbGlobalUser = GetCurrentGlobalUser(_user, _signInManager, _userManager,
                g => g.SantaUser, g => g.SantaUser.GiftingGroupLinks);

            Santa_GiftingGroupUser? dbGiftingGroupLink = dbGlobalUser?.SantaUser?.GiftingGroupLinks.FirstOrDefault(x => x.GiftingGroupId == Item.Id);

            if (dbGiftingGroupLink != null)
            {
                if (dbGiftingGroupLink.GroupAdmin)
                {
                    dbItem = dbGiftingGroupLink.GiftingGroup;

                    // TODO: Check that the name is unique if changed
                }
                else
                {
                    throw new AccessDeniedException();
                }
            }

            if (dbItem == null)
            {
                throw new NotFoundException("Gifting Group");
            }
        }
        else
        {
            // TODO: Check that the name, token etc.are unique

            dbItem = new Santa_GiftingGroup();
            ModelContext.Add(dbItem);
        }

        dbItem.Name = Item.Name;
        dbItem.Description = Item.Description;
        dbItem.JoinerToken = Item.JoinerToken;
        dbItem.CultureInfo = Item.CultureInfo;
        dbItem.CurrencyCodeOverride = Item.CurrencyCodeOverride;
        dbItem.CurrencySymbolOverride = Item.CurrencySymbolOverride;

        await ModelContext.SaveChangesAsync();
        Success = true;
        return await Result();
    }
}
