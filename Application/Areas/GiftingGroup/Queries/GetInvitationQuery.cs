using Application.Areas.GiftingGroup.BaseModels;
using Global.Abstractions.Areas.GiftingGroup;

namespace Application.Areas.GiftingGroup.Queries;

public class GetInvitationQuery : BaseQuery<IAcceptGroupInvitation?>
{
    private readonly string _invitationId;

    public GetInvitationQuery(string invitationId)
    {
        _invitationId = invitationId;
    }

    protected async override Task<IAcceptGroupInvitation?> Handle()
    {
        Santa_Invitation? dbInvitation = await Send(new Internal.GetInvitationEntityQuery(_invitationId));

        if (dbInvitation == null)
            return null;

        return new AcceptGroupInvitation
        {
            InvitationGuid = dbInvitation.InvitationGuid,
            ToSantaUserKey = dbInvitation.ToSantaUserKey,
            FromUser = Mapper.Map<IHashableUser>(dbInvitation.FromSantaUser.GlobalUser),
            Message = dbInvitation.Message
        };
    }
}
