using Global.Abstractions.Global.Partners;

namespace ViewLayer.Models.Partners;

public class RelationshipsVm : IRelationships
{
    public RelationshipsVm(List<RelationshipVm> possibleRelationships)
    {
        PossibleRelationships = possibleRelationships;
    }

    public List<RelationshipVm> PossibleRelationships { get; set; }
    IList<IRelationship> IRelationships.PossibleRelationships => PossibleRelationships.ToList<IRelationship>();

    public RelationshipVm? NewRelationship { get; set; }
}
