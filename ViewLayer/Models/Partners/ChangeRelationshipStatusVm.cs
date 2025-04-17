using Global.Abstractions.Areas.Partners;
using static Global.Settings.PartnerSettings;

namespace ViewLayer.Models.Partners;

public sealed class ChangeRelationshipStatusVm : IChangeRelationshipStatus
{
    public ChangeRelationshipStatusVm(int partnerLinkKey, Guid globalUserId, RelationshipStatus newStatus, string manageRelationshipsLink)
    {
        PartnerLinkKey = partnerLinkKey;
        GlobalUserId = globalUserId;
        NewStatus = newStatus;
        ManageRelationshipsLink = manageRelationshipsLink;
    }

    public int PartnerLinkKey { get; set; }
    public Guid GlobalUserId { get; set; }
    public RelationshipStatus NewStatus { get; set; }
    public string ManageRelationshipsLink { get; set; }
}
