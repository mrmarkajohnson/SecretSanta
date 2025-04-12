using Global.Abstractions.Global;

namespace Global.Abstractions.Areas.Partners;

public interface IAddRelationship : IRelationshipBase
{
    bool IsActive { get; }

    IQueryable<IVisibleUser> PossiblePartners { get; set; }
}
