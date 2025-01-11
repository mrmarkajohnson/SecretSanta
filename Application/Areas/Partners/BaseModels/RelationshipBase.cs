using Application.Shared.BaseModels;
using Global.Abstractions.Areas.Partners;
using Global.Settings;

namespace Application.Areas.Partners.BaseModels;

public abstract class RelationshipBase : IRelationship
{
    public int? PartnerLinkId { get; set; }
    public virtual bool SuggestedByCurrentUser { get; set; }

    public UserNamesBase Partner { get; set; } = new();
    IUserNamesBase IRelationship.Partner => Partner;

    public PartnerSettings.RelationshipStatus Status { get; set; }
    public IList<string> SharedGroupNames { get; set; } = new List<string>();
}
