using Global.Abstractions.Global;
using static Global.Settings.PartnerSettings;

namespace Global.Abstractions.Areas.Partners;

public interface IRelationship
{
    int? PartnerLinkKey { get; }

    /// <summary>
    /// Did the current user suggest the relationship (true), or did/must they confirm it (false)?
    /// </summary>
    bool SuggestedByCurrentUser { get; }

    IUserNamesBase Partner { get; }

    IList<string> SharedGroupNames { get; }
    RelationshipStatus Status { get; set; }
}
