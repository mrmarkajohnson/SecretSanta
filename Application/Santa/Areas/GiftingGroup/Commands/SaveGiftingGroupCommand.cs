using Application.Santa.Areas.GiftingGroup.Queries;
using Global.Abstractions.Santa.Areas.GiftingGroup;
using Global.Extensions.Exceptions;

namespace Application.Santa.Areas.GiftingGroup.Commands;

public class SaveGiftingGroupCommand<TItem> : BaseCommand<TItem> where TItem : IGiftingGroup
{
    public SaveGiftingGroupCommand(TItem item) : base(item)
    {
    }

    protected async override Task<ICommandResult<TItem>> HandlePostValidation()
    {
        Santa_GiftingGroup? dbGiftingGroup = null;

        Global_User dbCurrentUser = GetCurrentGlobalUser(g => g.SantaUser, g => g.SantaUser.GiftingGroupLinks);
        if (dbCurrentUser.SantaUser == null)
        {
            throw new AccessDeniedException();
        }

        if (Item.Id > 0)
        {
            Santa_GiftingGroupUser? dbGiftingGroupLink = dbCurrentUser.SantaUser.GiftingGroupLinks
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
        else if (dbCurrentUser?.SantaUser != null)
        {
            await ReplaceTokenIfNotUnique();

            dbGiftingGroup = new Santa_GiftingGroup();
            DbContext.Add(dbGiftingGroup);

            dbGiftingGroup.UserLinks.Add(new Santa_GiftingGroupUser
            {
                GroupAdmin = true,
                SantaUserId = dbCurrentUser.SantaUser.Id,
                SantaUser = dbCurrentUser.SantaUser,
                GiftingGroup = dbGiftingGroup
            });
        }

        if (dbGiftingGroup == null)
        {
            throw new NotFoundException("Gifting Group");
        }

        if (Item.Name != dbGiftingGroup.Name)
        {
            List<string> existingNames = DbContext.Santa_GiftingGroups.Select(x => x.Name).ToList();
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
            dbGiftingGroup.FirstYear = Item.FirstYear;

            return await SaveAndReturnSuccess();
        }

        return await Result();
    }

    private async Task ReplaceTokenIfNotUnique()
    {
        Item.JoinerToken = await Send(new GetUniqueJoinerTokenQuery(Item.JoinerToken));
    }
}
