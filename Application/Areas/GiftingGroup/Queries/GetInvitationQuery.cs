using Application.Areas.GiftingGroup.BaseModels;
using Global.Abstractions.Areas.GiftingGroup;
using Global.Extensions.Exceptions;

namespace Application.Areas.GiftingGroup.Queries;

public class GetInvitationQuery : BaseQuery<IReviewGroupInvitation>
{
    public GetInvitationQuery(string invitationId)
    {
        _invitationId = invitationId;
    }

    private readonly string _invitationId;

    protected async override Task<IReviewGroupInvitation> Handle()
    {
        Santa_Invitation? dbInvitation = await Send(new Internal.GetInvitationEntityQuery(_invitationId));

        if (dbInvitation == null)  // shouldn't happen, as the internal query will throw an exception if not returning the entity
        {
            throw new NotFoundException("invitation");
        }

        return new ReviewGroupInvitation
        {
            InvitationGuid = dbInvitation.InvitationGuid,
            ToSantaUserKey = dbInvitation.ToSantaUserKey,
            FromUser = Mapper.Map<IUserNamesBase>(dbInvitation.FromSantaUser),
            Message = dbInvitation.Message
        };
    }
}
