using Application.Santa.Areas.GiftingGroup.Queries;
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

        Santa_GiftingGroup? dbGiftingGroup = null;

        Global_User? dbGlobalUser = GetCurrentGlobalUser(_user, _signInManager, _userManager,
                g => g.SantaUser, g => g.SantaUser.GiftingGroupLinks);

        // TODO: Ensure the name is unique if new or changed

        if (Item.Id > 0)
        {
            Santa_GiftingGroupUser? dbGiftingGroupLink = dbGlobalUser?.SantaUser?.GiftingGroupLinks
                .Where(x => x.DateDeleted == null && x.GiftingGroup.DateDeleted == null)
                .FirstOrDefault(x => x.GiftingGroupId == Item.Id);

            if (dbGiftingGroupLink != null)
            {
                if (dbGiftingGroupLink.GroupAdmin)
                {
                    dbGiftingGroup = dbGiftingGroupLink.GiftingGroup;

                    if (Item.JoinerToken != dbGiftingGroup.JoinerToken)
                    {
                        await ReplaceTokenIfNotUnique();
                    }
                }
                else
                {
                    throw new AccessDeniedException();
                }
            }            
        }
        else if (dbGlobalUser?.SantaUser != null)
        {
            await ReplaceTokenIfNotUnique();

            dbGiftingGroup = new Santa_GiftingGroup();
            ModelContext.Add(dbGiftingGroup);

            dbGiftingGroup.UserLinks.Add(new Santa_GiftingGroupUser
            {
                GroupAdmin = true,
                UserId = dbGlobalUser.SantaUser.Id,
                User = dbGlobalUser.SantaUser,
                GiftingGroup = dbGiftingGroup
            });
        }

        if (dbGiftingGroup == null)
        {
            throw new NotFoundException("Gifting Group");
        }

        if (Item.Name != dbGiftingGroup.Name)
        {
            List<string> existingNames = ModelContext.Santa_GiftingGroups.Select(x => x.Name).ToList();
            if (existingNames.Contains(Item.Name.Trim()))
            {
                AddValidationError(nameof(Item.Name),  $"Another group with name '{Item.Name}' already exists. Names must be unique.");
            }
        }

        if (Validation.IsValid)
        {
            dbGiftingGroup.Name = Item.Name.Trim();
            dbGiftingGroup.Description = Item.Description.Trim();
            dbGiftingGroup.JoinerToken = Item.JoinerToken;
            dbGiftingGroup.CultureInfo = Item.CultureInfo;
            dbGiftingGroup.CurrencyCodeOverride = Item.CurrencyCodeOverride;
            dbGiftingGroup.CurrencySymbolOverride = Item.CurrencySymbolOverride;

            await ModelContext.SaveChangesAsync();
            Success = true;
        }

        return await Result();
    }

    private async Task ReplaceTokenIfNotUnique()
    {
        Item.JoinerToken = await Send(new GetUniqueJoinerTokenQuery(Item.JoinerToken));
    }
}
