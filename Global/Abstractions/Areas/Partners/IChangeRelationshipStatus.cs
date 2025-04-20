using static Global.Settings.PartnerSettings;

namespace Global.Abstractions.Areas.Partners;

public interface IChangeRelationshipStatus : IChangeRelationshipBase
{
    int PartnerLinkKey { get; }
    RelationshipStatus NewStatus { get; }
}
