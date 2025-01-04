using static Global.Settings.PartnerSettings;

namespace Global.Abstractions.Global.Partners;

public interface IChangeRelationshipStatus : IRelationshipBase
{
    int PartnerLinkId { get; }
    RelationshipStatus NewStatus { get; }
}
