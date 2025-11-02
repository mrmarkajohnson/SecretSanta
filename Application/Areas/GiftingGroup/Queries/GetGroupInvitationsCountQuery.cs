namespace Application.Areas.GiftingGroup.Queries;

public sealed class GetGroupInvitationsCountQuery : BaseQuery<int>
{
    protected override Task<int> Handle()
    {
        Santa_User dbCurrentSantaUser = GetCurrentSantaUser();

        var dbOpenInvitations = dbCurrentSantaUser.ReceivedInvitations
            .Where(x => x.DateArchived == null)
            .ToList();

        return Result(dbOpenInvitations.Count());
    }
}
