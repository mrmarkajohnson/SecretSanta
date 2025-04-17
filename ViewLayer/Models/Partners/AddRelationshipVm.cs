using Global.Abstractions.Areas.Partners;

namespace ViewLayer.Models.Partners;

public sealed class AddRelationshipVm : IAddRelationship
{
    public AddRelationshipVm(IQueryable<IVisibleUser> possiblePartners, string manageRelationshipsLink, string userGridAction)
    {
        PossiblePartners = possiblePartners;
        ManageRelationshipsLink = manageRelationshipsLink;
        UserGridAction = userGridAction;
    }

    public IQueryable<IVisibleUser> PossiblePartners { get; set; }
    public string ManageRelationshipsLink { get; set; }
    public string UserGridAction { get; }
    public Guid GlobalUserId { get; set; } = Guid.Empty;
    public bool IsActive { get; set; }

    public UserGridVm UserGridModel => new UserGridVm(PossiblePartners, UserGridAction);    
}
