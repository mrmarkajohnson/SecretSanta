using Global.Abstractions.Global.Shared;

namespace Global.Abstractions.Global.Partners;

public interface IAddRelationship : IRelationshipBase
{
    IQueryable<IVisibleUser> PossiblePartners { get; set; }
}
