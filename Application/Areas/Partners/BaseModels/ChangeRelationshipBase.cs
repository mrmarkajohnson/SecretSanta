using Global.Abstractions.Areas.Partners;

namespace Application.Areas.Partners.BaseModels;

public class ChangeRelationshipBase : IChangeRelationshipBase
{
    public ChangeRelationshipBase(string hashedUserId, string manageRelationshipsLink)
    {
        HashedUserId = hashedUserId;
        ManageRelationshipsLink = manageRelationshipsLink;
    }

    public string HashedUserId { get; set; }
    public string ManageRelationshipsLink { get; set; }
}
