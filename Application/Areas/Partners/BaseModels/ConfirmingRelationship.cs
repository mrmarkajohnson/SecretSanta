using Global.Abstractions.Areas.Partners;

namespace Application.Areas.Partners.BaseModels;

/// <summary>
/// Relationship confirmed by, or to be confirmed by, the end user
/// </summary>
internal class ConfirmingRelationship : RelationshipBase, IRelationship
{
    public override bool SuggestedByCurrentUser => false;
}
