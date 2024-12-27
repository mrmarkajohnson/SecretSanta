using Global.Abstractions.Global.Shared;
using static Global.Settings.PartnerSettings;

namespace Global.Abstractions.Global.Partners;

public interface IRelationship
{
    int? PartnerLinkId { get; }
    
    /// <summary>
    /// Did the current user suggest the relationship (true), or did/must they confirm it (false)?
    /// </summary>
    bool SuggestedByCurrentUser { get; }

    IUserNamesBase Parter { get;  }

    IList<string> SharedGroupNames { get; }
    RelationshipStatus Status { get; set; }
}
