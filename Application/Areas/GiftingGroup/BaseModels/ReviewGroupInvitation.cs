using Global.Abstractions.Areas.GiftingGroup;
using System.ComponentModel.DataAnnotations;
using static Global.Settings.GlobalSettings;

namespace Application.Areas.GiftingGroup.BaseModels;

public class ReviewGroupInvitation : IReviewGroupInvitation
{
    public Guid InvitationGuid { get; set; }
    public int GiftingGroupKey { get; set; }
    public int? ToSantaUserKey { get; set; }

    [Display(Name = "Group Name")]
    public required string GroupName { get; set; }

    [Display(Name = "Group Description")]
    public required string GroupDescription { get; set; }

    [Display(Name = "From")]
    public required IUserNamesBase FromUser { get; set; }

    public string? InvitationMessage { get; set; }
    public YesNoNotSure? Accept { get; set; }

    [Display(Name = "Explanation")]
    [MaxLength(GiftingGroupVal.RejectInvitationMessage.MaxLength)]
    public string? RejectionMessage { get; set; }
}
