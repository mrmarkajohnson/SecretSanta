using Global.Abstractions.Shared;

namespace Global.Abstractions.Areas.Partners;

public interface IAddRelationship : IChangeRelationshipBase
{
    bool IsActive { get; }

    IQueryable<IVisibleUser> PossiblePartners { get; set; }
}
