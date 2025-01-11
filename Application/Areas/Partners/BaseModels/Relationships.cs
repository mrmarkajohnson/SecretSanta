using Global.Abstractions.Areas.Partners;

namespace Application.Areas.Partners.BaseModels;

public class Relationships : IRelationships
{
    public Relationships()
    {
        PossibleRelationships = new List<IRelationship>();
    }

    public IList<IRelationship> PossibleRelationships { get; set; }
}
