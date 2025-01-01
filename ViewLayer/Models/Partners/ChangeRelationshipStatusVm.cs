using Global.Abstractions.Global.Partners;
using static Global.Settings.PartnerSettings;

namespace ViewLayer.Models.Partners;

public class ChangeRelationshipStatusVm : IChangeRelationshipStatus
{
    public ChangeRelationshipStatusVm(int partnerLinkId, Guid userId, RelationshipStatus newStatus)
    {
        PartnerLinkId = partnerLinkId;
        UserId = userId;
        NewStatus = newStatus;
    }

    public int PartnerLinkId { get; set; }
    public Guid UserId { get; set; }
    public RelationshipStatus NewStatus { get; set; }
}
