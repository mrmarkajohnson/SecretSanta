using Application.Shared.Requests;
using Global.Abstractions.Areas.GiftingGroup;
using Global.Extensions.Exceptions;

namespace Application.Areas.GiftingGroup.Commands;

public sealed class ReviewJoinerApplicationCommand<TItem> : BaseCommand<TItem> where TItem : IReviewApplication
{
    public ReviewJoinerApplicationCommand(TItem item) : base(item)
    {
    }

    protected async override Task<ICommandResult<TItem>> HandlePostValidation()
    {
        Santa_User dbCurrentSantaUser = GetCurrentSantaUser(s => s.GiftingGroupLinks);

        var dbApplication = dbCurrentSantaUser.GiftingGroupLinks
            .Where(x => x.DateDeleted == null && x.GiftingGroup != null && x.GiftingGroup.DateDeleted == null && x.GroupAdmin)
            .Select(x => x.GiftingGroup)
            .SelectMany(x => x.MemberApplications)
            .FirstOrDefault(x => x.GroupApplicationKey == Item.GroupApplicationKey);

        if (dbApplication == null)
        {
            dbApplication = DbContext.Santa_GiftingGroupApplications.FirstOrDefault(x => x.GroupApplicationKey == Item.GroupApplicationKey);

            if (dbApplication != null && dbApplication.GiftingGroup.DateDeleted == null)
            {
                var dbLinks = dbCurrentSantaUser.GiftingGroupLinks
                    .Where(x => x.GiftingGroupKey == dbApplication.GiftingGroupKey && x.GroupAdmin)
                    .ToList();

                if (!dbLinks.Any() == true)
                {
                    throw new AccessDeniedException();
                }
            }

            throw new NotFoundException("Application");
        }

        if (Validation.IsValid && dbApplication != null)
        {
            dbApplication.Accepted = Item.Accepted;
            dbApplication.RejectionMessage = Item.Accepted ? null : Item.RejectionMessage;
            dbApplication.Blocked = Item.Accepted ? false : Item.Blocked;
            dbApplication.ResponseBySantaUserKey = dbCurrentSantaUser.SantaUserKey;

            if (Item.Accepted)
            {
                AddToGiftingGroup(dbApplication);
                AddToCurrentYear(dbApplication);
            }

            return await SaveAndReturnSuccess();
        }

        return await Result();
    }

    private static void AddToGiftingGroup(Santa_GiftingGroupApplication dbApplication)
    {
        dbApplication.GiftingGroup.UserLinks.Add(new Santa_GiftingGroupUser
        {
            GiftingGroup = dbApplication.GiftingGroup,
            GiftingGroupKey = dbApplication.GiftingGroupKey,
            SantaUser = dbApplication.SantaUser,
            SantaUserKey = dbApplication.SantaUserKey,
        });
    }

    private void AddToCurrentYear(Santa_GiftingGroupApplication dbApplication)
    {
        bool alreadyCalculated = Item.CurrentYearCalculated;
        var dbGiftingGroupYear = dbApplication.GiftingGroup.Years.FirstOrDefault(x => x.CalendarYear == DateTime.Today.Year);

        if (!alreadyCalculated) // just in case
        {
            if (dbGiftingGroupYear != null)
            {
                alreadyCalculated = dbGiftingGroupYear.Users.Any(x => x.RecipientSantaUserKey != null);
            }
        }

        if (!alreadyCalculated && dbGiftingGroupYear != null)
        {
            dbGiftingGroupYear.Users.Add(new Santa_YearGroupUser
            {
                GiftingGroupYearKey = dbGiftingGroupYear.CalendarYear,
                GiftingGroupYear = dbGiftingGroupYear,
                SantaUserKey = dbApplication.SantaUserKey,
                SantaUser = dbApplication.SantaUser,
                Included = true
            });
        }
    }
}
