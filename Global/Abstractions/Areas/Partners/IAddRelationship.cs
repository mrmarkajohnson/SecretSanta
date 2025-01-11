using Global.Abstractions.Global;

namespace Global.Abstractions.Areas.Partners;

public interface IAddRelationship : IRelationshipBase
{
    IQueryable<IVisibleUser> PossiblePartners { get; set; }
}
