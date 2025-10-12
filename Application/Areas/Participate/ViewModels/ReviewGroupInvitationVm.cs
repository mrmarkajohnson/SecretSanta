using Application.Areas.GiftingGroup.BaseModels;
using Global.Abstractions.Areas.GiftingGroup;
using Global.Abstractions.ViewModels;

namespace Application.Areas.Participate.ViewModels;

public class ReviewGroupInvitationVm : ReviewGroupInvitation, IReviewGroupInvitation, IFormVm
{
    public string? ReturnUrl { get; set; }
    public string SubmitButtonText { get; set; } = "Save";
    public string SubmitButtonIcon { get; set; } = "fa-save";
    public string? SuccessMessage { get; set; }
}
