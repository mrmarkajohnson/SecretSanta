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
            .Where(GroupUserExpressions.IsActive(true))
            .Where(x => x.GroupAdmin)
            .Select(x => x.GiftingGroup)
                .SelectMany(x => x.MemberApplications)
                    .Where(GroupApplicationExpressions.IsActive(false))
                    .FirstOrDefault(x => x.GroupApplicationKey == _groupApplicationKey);

        if (dbApplication == null) // then check if it exists but the user doesn't have admin access
        {
            dbApplication = DbContext.Santa_GiftingGroupApplications
                .Where(GroupApplicationExpressions.IsActive(true))
                .FirstOrDefault(x => x.GroupApplicationKey == _groupApplicationKey);

            if (dbApplication != null)
            {
                var dbLinks = dbCurrentSantaUser.GiftingGroupLinks
                    .Where(GroupUserExpressions.IsActive(true))
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
