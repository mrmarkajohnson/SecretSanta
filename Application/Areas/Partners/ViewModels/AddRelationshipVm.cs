using Application.Areas.Partners.BaseModels;
using Application.Shared.ViewModels;
using Global.Abstractions.Areas.Partners;

namespace Application.Areas.Partners.ViewModels;

public sealed class AddRelationshipVm : ChangeRelationshipBase, IAddRelationship
{
    public AddRelationshipVm(IQueryable<IVisibleUser> possiblePartners, string hashedUserId, string manageRelationshipsLink, string userGridAction)
        : base(hashedUserId, manageRelationshipsLink)
    {
        PossiblePartners = possiblePartners;
        UserGridAction = userGridAction;
    }

    public IQueryable<IVisibleUser> PossiblePartners { get; set; }
    public string UserGridAction { get; }
    public bool IsActive { get; set; }

    public UserGridVm UserGridModel => new UserGridVm(PossiblePartners, UserGridAction);
}
