using Application.Areas.Account.Actions;
using Application.Shared.Requests;
using Global.Abstractions.Areas.GiftingGroup;
using Global.Extensions.Exceptions;

namespace Application.Areas.GiftingGroup.Queries;

public class ReviewJoinerApplicationQuery : BaseQuery<IReviewApplication>
{
    private int _applicationId;

    public ReviewJoinerApplicationQuery(int applicationId)
    {
        _applicationId = applicationId;
    }

    protected async override Task<IReviewApplication> Handle()
    {
        Santa_User dbCurrentSantaUser = GetCurrentSantaUser(s => s.GiftingGroupLinks);

        var dbApplication = dbCurrentSantaUser.GiftingGroupLinks
            .Where(x => x.DateDeleted == null && x.GiftingGroup != null && x.GiftingGroup.DateDeleted == null && x.GroupAdmin)
            .Select(x => x.GiftingGroup)
            .SelectMany(x => x.MemberApplications)
            .FirstOrDefault(x => x.Id == _applicationId);

        if (dbApplication == null)
        {
            dbApplication = DbContext.Santa_GiftingGroupApplications.FirstOrDefault(x => x.Id == _applicationId);

            if (dbApplication != null && dbApplication.GiftingGroup.DateDeleted == null)
            {
                var dbLinks = dbCurrentSantaUser.GiftingGroupLinks
                    .Where(x => x.GiftingGroupId == dbApplication.GiftingGroupId && x.GroupAdmin)
                    .ToList();

                if (!dbLinks?.Any() == true)
                {
                    throw new AccessDeniedException();
                }
            }

            throw new NotFoundException("Application");
        }

        var application = Mapper.Map<IReviewApplication>(dbApplication);
        await Send(new UnHashUserIdentificationAction(application));

        return application;
    }
}
