using Application.Areas.GiftingGroup.BaseModels;
using Application.Shared.Requests;
using Global.Abstractions.Areas.Account;
using Global.Abstractions.Areas.GiftingGroup;

namespace Application.Areas.GiftingGroup.Queries;

public class GetInvitationQuery : BaseQuery<IGiftingGroupInvitation?>
{
    private readonly string _invitationId;

    public GetInvitationQuery(string invitationId)
    {
        _invitationId = invitationId;
    }

    protected async override Task<IGiftingGroupInvitation?> Handle()
    {
        Santa_Invitation? dbInvitation = await Send(new Internal.GetInvitationEntityQuery(_invitationId));

        if (dbInvitation == null)
            return null;

        return new GiftingGroupInvitation
        {
            InvitationGuid = dbInvitation.InvitationGuid,
            ToSantaUserKey = dbInvitation.ToSantaUserKey,
            FromUser = Mapper.Map<IHashableUser>(dbInvitation.FromSantaUser.GlobalUser)
        };
    }
}
