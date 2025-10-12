using Global.Abstractions.Areas.GiftingGroup;

namespace Application.Areas.GiftingGroup.BaseModels;

public class ReviewGroupInvitation : IReviewGroupInvitation
{
    public Guid InvitationGuid { get; set; }
    public int? ToSantaUserKey { get; set; }
    public required IUserNamesBase FromUser { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool? Accept { get; set; }
    public string? RejectionMessage { get; set; }
}
