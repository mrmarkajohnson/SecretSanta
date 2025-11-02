using Global.Abstractions.Areas.GiftingGroup;

namespace Application.Areas.GiftingGroup.ViewModels;

public class GroupInvitationsVm
{
    public IQueryable<IReviewGroupInvitation> Invitations { get; set; } = new List<IReviewGroupInvitation>().AsQueryable();
    public int InvitationsCount => Invitations.Count();
}
