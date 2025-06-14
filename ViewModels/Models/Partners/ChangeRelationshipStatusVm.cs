using Application.Areas.Partners.BaseModels;
using Global.Abstractions.Areas.Partners;
using static Global.Settings.PartnerSettings;

namespace ViewModels.Models.Partners;

public sealed class ChangeRelationshipStatusVm : ChangeRelationshipBase, IChangeRelationshipStatus
{
    public ChangeRelationshipStatusVm(int partnerLinkKey, string hashedUserId, RelationshipStatus newStatus, string manageRelationshipsLink)
        : base(hashedUserId, manageRelationshipsLink)
    {
        PartnerLinkKey = partnerLinkKey;
        NewStatus = newStatus;
    }

    public int PartnerLinkKey { get; set; }
    public RelationshipStatus NewStatus { get; set; }
}
