using Global.Abstractions.Areas.GiftingGroup;
using Global.Extensions.Exceptions;

namespace Application.Areas.GiftingGroup.Queries;

public class GetInvitationQuery : BaseQuery<IReviewGroupInvitation>
{
    public GetInvitationQuery(string invitationId)
    {
        _invitationId = invitationId;
    }

    public GetInvitationQuery(Guid? invitationGuid = null)
    {
        _invitationId = string.Empty;
        _invitationGuid = invitationGuid;
    }

    private readonly string _invitationId;
    private readonly Guid? _invitationGuid;

    protected async override Task<IReviewGroupInvitation> Handle()
    {
        Santa_Invitation? dbInvitation = _invitationGuid != null 
            ? await Send(new Internal.GetInvitationEntitySavingQuery(_invitationGuid.Value))
            : await Send(new Internal.GetInvitationEntitySavingQuery(_invitationId));

        if (dbInvitation == null)  // shouldn't happen, as the internal query will throw an exception if not returning the entity
        {
            throw new NotFoundException("invitation");
        }

        var invitation = Mapper.Map<IReviewGroupInvitation>(dbInvitation);
        invitation.FromUser.UnHash();
        return invitation;
    }
}
