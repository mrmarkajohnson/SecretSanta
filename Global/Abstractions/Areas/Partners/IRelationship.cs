using Global.Abstractions.Shared;
using static Global.Settings.PartnerSettings;

namespace Global.Abstractions.Areas.Partners;

public interface IRelationship
{
    int? PartnerLinkKey { get; }

    /// <summary>
    /// Did the current user suggest the relationship (true), or did/must they confirm it (false)?
    /// </summary>
    bool SuggestedByCurrentUser { get; }

    /// <summary>
    /// Null means awaiting confirmation, false means never in a relationship
    /// </summary>
    bool? AlreadyConfirmed { get; set; }

    IUserNamesBase Partner { get; }

    IList<string> SharedGroupNames { get; }
    RelationshipStatus Status { get; set; }
}
