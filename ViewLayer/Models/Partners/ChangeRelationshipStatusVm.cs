using Global.Abstractions.Areas.Partners;
using static Global.Settings.PartnerSettings;

namespace ViewLayer.Models.Partners;

public class ChangeRelationshipStatusVm : IChangeRelationshipStatus
{
    public ChangeRelationshipStatusVm(int partnerLinkId, Guid userId, RelationshipStatus newStatus, string manageRelationshipsLink)
    {
        PartnerLinkId = partnerLinkId;
        UserId = userId;
        NewStatus = newStatus;
        ManageRelationshipsLink = manageRelationshipsLink;
    }

    public int PartnerLinkId { get; set; }
    public Guid UserId { get; set; }
    public RelationshipStatus NewStatus { get; set; }
    public string ManageRelationshipsLink { get; set; }
}
