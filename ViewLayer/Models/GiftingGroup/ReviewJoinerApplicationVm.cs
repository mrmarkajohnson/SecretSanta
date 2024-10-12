using Application.Santa.Areas.GiftingGroup.BaseModels;
using Global.Abstractions.Santa.Areas.GiftingGroup;

namespace ViewLayer.Models.GiftingGroup;

public class ReviewJoinerApplicationVm : ReviewJoinerApplication, IForm, IReviewApplication
{
    public string? ReturnUrl { get; set; }
    public string SubmitButtonText { get; set; } = "Submit";
    public string SubmitButtonIcon { get; set; } = "fa-paper-plane";
    public string? SuccessMessage { get; set; }
}
