using Global.Abstractions.Global.Partners;
using Global.Abstractions.Global.Shared;

namespace ViewLayer.Models.Partners;

internal class AddRelationshipVm : IAddRelationship
{
    public AddRelationshipVm(IQueryable<IVisibleUser> possiblePartners, string manageRelationshipsLink)
    {
        PossiblePartners = possiblePartners;
        ManageRelationshipsLink = manageRelationshipsLink;
    }

    public IQueryable<IVisibleUser> PossiblePartners { get; set; }
    public string ManageRelationshipsLink { get; set; }
    public Guid UserId { get; set; } = Guid.Empty;
}
