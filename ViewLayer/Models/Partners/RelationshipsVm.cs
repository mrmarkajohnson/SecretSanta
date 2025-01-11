using Global.Abstractions.Areas.Partners;

namespace ViewLayer.Models.Partners;

public class RelationshipsVm : BasePageVm, IRelationships
{
    public RelationshipsVm(List<RelationshipVm> possibleRelationships)
    {
        PossibleRelationships = possibleRelationships;
    }

    public List<RelationshipVm> PossibleRelationships { get; set; }
    IList<IRelationship> IRelationships.PossibleRelationships => PossibleRelationships.ToList<IRelationship>();
}
