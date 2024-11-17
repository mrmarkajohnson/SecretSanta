using Application.Santa.Areas.GiftingGroup.BaseModels;
using Global.Abstractions.Santa.Areas.GiftingGroup;

namespace ViewLayer.Models.GiftingGroup;

public class SetGiftingGroupYearVm : GiftingGroupYear, IGiftingGroupYear, IForm
{
    public string? ReturnUrl { get; set; }
    public string SubmitButtonText { get; set; } = "Save";
    public string SubmitButtonIcon { get; set; } = "fa-save";
    public string? SuccessMessage { get; set; }
}
