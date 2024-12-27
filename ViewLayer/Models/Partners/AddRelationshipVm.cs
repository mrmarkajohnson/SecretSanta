using Global.Abstractions.Global.Shared;

namespace ViewLayer.Models.Partners;

internal class AddRelationshipVm
{
    public IList<IVisibleUser> VisibleUsers { get; set; }

    public AddRelationshipVm(IList<IVisibleUser> visibleUsers)
    {
        VisibleUsers = visibleUsers;
    }
}
