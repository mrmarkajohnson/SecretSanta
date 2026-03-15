using AutoMapper.QueryableExtensions;
using Global.Abstractions.Areas.GiftingGroup;

namespace Application.Areas.GiftingGroup.Queries;

public sealed class GetGroupInvitationsQuery : BaseQuery<IQueryable<IReviewGroupInvitation>>
{
    protected override Task<IQueryable<IReviewGroupInvitation>> Handle()
    {
        Santa_User dbCurrentSantaUser = GetCurrentSantaUser();

        var dbOpenInvitations = dbCurrentSantaUser.ReceivedInvitations
            .Where(GroupInvitationExpressions.IsActive(true))
            .ToList();

        return Result(dbOpenInvitations.AsQueryable().ProjectTo<IReviewGroupInvitation>(Mapper.ConfigurationProvider));
    }
}