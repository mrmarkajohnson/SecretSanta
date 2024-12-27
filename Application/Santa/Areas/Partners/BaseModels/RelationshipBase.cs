using Application.Santa.Areas.GiftingGroup.BaseModels;
using Global.Abstractions.Global.Partners;
using Global.Abstractions.Global.Shared;
using Global.Settings;

namespace Application.Santa.Areas.Partners.BaseModels;

public abstract class RelationshipBase : IRelationship
{
    public int? PartnerLinkId { get; set; }
    public virtual bool SuggestedByCurrentUser { get; set; }

    public UserNamesBase Partner { get; set; } = new();
    IUserNamesBase IRelationship.Parter => Partner;

    public PartnerSettings.RelationshipStatus Status { get; set; }
    public IList<string> SharedGroupNames { get; set; } = new List<string>();
}
