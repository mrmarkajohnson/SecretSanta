using Application.Shared.ViewModels;
using Global.Abstractions.Areas.Partners;

namespace Application.Areas.Partners.ViewModels;

public sealed class RelationshipsVm : BasePageVm, IRelationships
{
    public RelationshipsVm(List<RelationshipVm> possibleRelationships)
    {
        PossibleRelationships = possibleRelationships;
    }

    public List<RelationshipVm> PossibleRelationships { get; set; }
    IList<IRelationship> IRelationships.PossibleRelationships => PossibleRelationships.ToList<IRelationship>();
}
