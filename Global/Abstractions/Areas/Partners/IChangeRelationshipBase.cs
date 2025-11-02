using Global.Abstractions.Shared;

namespace Global.Abstractions.Areas.Partners;

public interface IChangeRelationshipBase : IHaveAHashedUserId
{
    string ManageRelationshipsLink { get; }
}
