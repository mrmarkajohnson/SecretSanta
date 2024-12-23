using Global.Abstractions.Santa.Areas.GiftingGroup;
using Global.Extensions.Exceptions;

namespace Application.Santa.Areas.GiftingGroup.Commands;

public class ReviewJoinerApplicationCommand<TItem> : BaseCommand<TItem> where TItem : IReviewApplication
{
    public ReviewJoinerApplicationCommand(TItem item) : base(item)
    {
    }

    protected async override Task<ICommandResult<TItem>> HandlePostValidation()
    {
        Global_User? dbCurrentUser = GetCurrentGlobalUser(g => g.SantaUser, g => g.SantaUser.GiftingGroupLinks);
        if (dbCurrentUser == null || dbCurrentUser.SantaUser == null)
        {
            throw new AccessDeniedException();
        }

        var dbApplication = dbCurrentUser.SantaUser?.GiftingGroupLinks
            .Where(x => x.DateDeleted == null && x.GiftingGroup != null && x.GiftingGroup.DateDeleted == null && x.GroupAdmin)
            .Select(x => x.GiftingGroup)
            .SelectMany(x => x.MemberApplications)
            .FirstOrDefault(x => x.Id == Item.ApplicationId);

        if (dbApplication == null)
        {
            dbApplication = DbContext.Santa_GiftingGroupApplications.FirstOrDefault(x => x.Id == Item.ApplicationId);

            if (dbApplication != null && dbApplication.GiftingGroup.DateDeleted == null)
            {
                var dbLinks = dbCurrentUser.SantaUser?.GiftingGroupLinks
                    .Where(x => x.GiftingGroupId == dbApplication.GiftingGroupId && x.GroupAdmin)
                    .ToList();

                if (!dbLinks?.Any() == true)
                {
                    throw new AccessDeniedException();
                }
            }

            throw new NotFoundException("application");
        }

        if (Validation.IsValid && dbApplication != null)
        {
            dbApplication.Accepted = Item.Accepted;
            dbApplication.RejectionMessage = Item.Accepted ? null : Item.RejectionMessage;
            dbApplication.Blocked = Item.Accepted ? false : Item.Blocked;
            dbApplication.ResponseByUserId = dbCurrentUser.SantaUser?.Id;

            AddToCurrentYear(dbApplication);

            return await SaveAndReturnSuccess();
        }

        return await Result();
    }

    private void AddToCurrentYear(Santa_GiftingGroupApplication dbApplication)
    {
        bool alreadyCalculated = Item.CurrentYearCalculated;
        var dbGiftingGroupYear = dbApplication.GiftingGroup.Years.FirstOrDefault(x => x.Year == DateTime.Today.Year);

        if (!alreadyCalculated) // just in case
        {
            if (dbGiftingGroupYear != null)
            {
                alreadyCalculated = dbGiftingGroupYear.Users.Any(x => x.GivingToUserId != null);
            }
        }

        if (!alreadyCalculated && dbGiftingGroupYear != null)
        {
            dbGiftingGroupYear.Users.Add(new Santa_YearGroupUser
            {
                YearId = dbGiftingGroupYear.Year,
                Year = dbGiftingGroupYear,
                SantaUserId = dbApplication.SantaUserId,
                SantaUser = dbApplication.SantaUser,
                Included = true
            });
        }
    }
}
