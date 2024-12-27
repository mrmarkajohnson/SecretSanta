using Global.Abstractions.Global.Partners;

namespace Application.Santa.Areas.Partners.BaseModels;

/// <summary>
/// Relationship confirmed by, or to be confirmed by, the end user
/// </summary>
internal class ConfirmingRelationship : RelationshipBase, IRelationship
{
    public override bool SuggestedByCurrentUser => false;
}
