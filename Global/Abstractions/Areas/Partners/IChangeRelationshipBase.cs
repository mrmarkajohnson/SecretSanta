using Global.Abstractions.Shared;

namespace Global.Abstractions.Areas.Partners;

public interface IChangeRelationshipBase : IHasHashedUserId
{
    string ManageRelationshipsLink { get; }
}
