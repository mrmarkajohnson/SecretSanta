using Application.Shared.Requests;
using Global.Abstractions.Areas.GiftingGroup;

namespace Application.Areas.GiftingGroup.Queries.Internal;

internal class GetInvitationEntityQuery : BaseQuery<Santa_Invitation?>
{
    private readonly string _invitationId;

    public GetInvitationEntityQuery(string invitationId)
    {
        _invitationId = invitationId;
    }

    protected override Task<Santa_Invitation?> Handle()
    {
        var dbOpenInvitations = DbContext.Santa_Invitations
            .Where(x => x.DateArchived == null)
            .ToList();

        Santa_Invitation? dbInvitation = dbOpenInvitations
            .FirstOrDefault(x => x.GetInvitationId() == _invitationId);

        return Result(dbInvitation);
    }
}
