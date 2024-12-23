using Application.Santa.Areas.Account.Actions;
using Global.Abstractions.Santa.Areas.GiftingGroup;
using Global.Extensions.Exceptions;

namespace Application.Santa.Areas.GiftingGroup.Queries;

public class ReviewJoinerApplicationQuery : BaseQuery<IReviewApplication>
{
    private int _applicationId;

    public ReviewJoinerApplicationQuery(int applicationId)
    {
        _applicationId = applicationId;
    }

    protected async override Task<IReviewApplication> Handle()
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
            .FirstOrDefault(x => x.Id == _applicationId);

        if (dbApplication == null)
        {
            dbApplication = DbContext.Santa_GiftingGroupApplications.FirstOrDefault(x => x.Id == _applicationId);

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

        var application = Mapper.Map<IReviewApplication>(dbApplication);
        await Send(new UnHashUserIdentificationAction(application));

        return application;
    }
}
