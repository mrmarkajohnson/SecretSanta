using Global.Abstractions.Areas.GiftingGroup;
using Global.Extensions.Exceptions;

namespace Application.Areas.GiftingGroup.Commands;

public class ParticipateInYearCommand<TItem> : GiftingGroupYearBaseCommand<TItem> where TItem : IUserGiftingGroupYear
{
    public ParticipateInYearCommand(TItem item) : base(item)
    {
    }

    protected async override Task<ICommandResult<TItem>> HandlePostValidation()
    {
        Santa_User dbSantaUser = GetCurrentSantaUser(s => s.GiftingGroupLinks);

        var dbGiftingGroupLink = dbSantaUser.GiftingGroupLinks
                .FirstOrDefault(x => x.DateDeleted == null && x.GiftingGroupId == Item.GiftingGroupId && x.GiftingGroup.DateDeleted == null);

        if (dbGiftingGroupLink == null)
            throw new NotFoundException($"Gifting Group '{Item.GiftingGroupName}'");

        var dbGiftingGroupYear = dbGiftingGroupLink.GiftingGroup.Years
            .FirstOrDefault(y => y.Year == Item.Year);

        if (dbGiftingGroupYear == null)
        {
            dbGiftingGroupYear = CreateGiftingGroupYear(dbGiftingGroupLink.GiftingGroup);
        }

        AddOrUpdateUserGroupYear(dbGiftingGroupYear, Item.Included, dbSantaUser.Id, dbSantaUser.GlobalUser.FullName(), dbSantaUser);

        return await SaveAndReturnSuccess();
    }
}
