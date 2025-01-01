using static Global.Settings.PartnerSettings;

namespace Global.Abstractions.Global.Partners;

public interface IChangeRelationshipStatus
{
    int PartnerLinkId { get; set; }
    Guid UserId { get; set; }
    RelationshipStatus NewStatus { get; set; }
}
