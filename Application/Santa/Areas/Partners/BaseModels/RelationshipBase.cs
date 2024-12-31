using Global.Abstractions.Global.Partners;
using Global.Abstractions.Global.Shared;
using Global.Settings;

namespace Application.Santa.Areas.Partners.BaseModels;

public abstract class RelationshipBase : IRelationship
{
    public int? PartnerLinkId { get; set; }
    public virtual bool SuggestedByCurrentUser { get; set; }

    public UserNamesBase Partner { get; set; } = new();
    IUserNamesBase IRelationship.Partner => Partner;

    public PartnerSettings.RelationshipStatus Status { get; set; }
    public IList<string> SharedGroupNames { get; set; } = new List<string>();
}
