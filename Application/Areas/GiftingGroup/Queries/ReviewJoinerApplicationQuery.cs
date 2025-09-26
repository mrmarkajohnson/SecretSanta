using Global.Abstractions.Areas.GiftingGroup;
using Global.Extensions.Exceptions;

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
            .Where(x => x.DateDeleted == null && x.GiftingGroup != null && x.GiftingGroup.DateDeleted == null && x.GroupAdmin)
            .Select(x => x.GiftingGroup)
            .SelectMany(x => x.MemberApplications)
            .FirstOrDefault(x => x.GroupApplicationKey == _groupApplicationKey);

        if (dbApplication == null)
        {
            dbApplication = DbContext.Santa_GiftingGroupApplications.FirstOrDefault(x => x.GroupApplicationKey == _groupApplicationKey);

            if (dbApplication != null && dbApplication.GiftingGroup.DateDeleted == null)
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
