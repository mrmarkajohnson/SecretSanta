using Global.Abstractions.Areas.Partners;

namespace Application.Areas.Partners.BaseModels;

/// <summary>
/// Relationship suggested by the current user
/// </summary>
internal class SuggestedRelationship : RelationshipBase, IRelationship
{
    public override bool SuggestedByCurrentUser => true;
}
