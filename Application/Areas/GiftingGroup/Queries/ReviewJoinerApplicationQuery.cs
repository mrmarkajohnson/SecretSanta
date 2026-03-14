using Global.Abstractions.Areas.GiftingGroup;
using Global.Extensions.Exceptions;
using Microsoft.AspNetCore.Builder;

namespace Application.Areas.GiftingGroup.Queries;

public sealed class ReviewJoinerApplicationQuery : BaseQuery<IReviewApplication>
{
    private int _groupApplicationKey;

    public ReviewJoinerApplicationQuery(int groupApplicationKey)
    {
        _groupApplicationKey = groupApplicationKey;
    }

    protected override Task<IReviewApplication> Handle()
    {
        Santa_User dbCurrentSantaUser = GetCurrentSantaUser(s => s.GiftingGroupLinks);

        var dbApplication = dbCurrentSantaUser.GiftingGroupLinks
            .Where(x => x.DateArchived == null && x.GiftingGroup != null && x.GiftingGroup.DateArchived == null && x.GroupAdmin)
            .Select(x => x.GiftingGroup)
            .SelectMany(x => x.MemberApplications)
            .Where(x => x.DateArchived == null)
            .FirstOrDefault(x => x.GroupApplicationKey == _groupApplicationKey);

        if (dbApplication == null)
        {
            dbApplication = DbContext.Santa_GiftingGroupApplications
                .Where(x => x.DateArchived == null && x.GiftingGroup.DateArchived == null)
                .FirstOrDefault(x => x.GroupApplicationKey == _groupApplicationKey);

            if (dbApplication != null)
            {
                var dbLinks = dbCurrentSantaUser.GiftingGroupLinks
                    .Where(x => x.GiftingGroupKey == dbApplication.GiftingGroupKey && x.GroupAdmin)
                    .ToList();

                if (!dbLinks?.Any() == true)
                {
                    throw new AccessDeniedException();
                }
            }

            throw new NotFoundException("Application");
        }

        var application = Mapper.Map<IReviewApplication>(dbApplication).UnHash();

        return Result(application);
    }
}
