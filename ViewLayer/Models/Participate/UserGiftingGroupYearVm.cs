using Application.Areas.GiftingGroup.BaseModels;
using FluentValidation;
using Global.Abstractions.Areas.GiftingGroup;

namespace ViewLayer.Models.Participate;

public class UserGiftingGroupYearVm : UserGiftingGroupYear, IUserGiftingGroupYear, IForm
{
    public string? ReturnUrl { get; set; }
    public string SubmitButtonText { get; set; } = "Save";
    public string SubmitButtonIcon { get; set; } = "fa-save";
    public string? SuccessMessage { get; set; }
}

public class UserGiftingGroupYearVmValidator : AbstractValidator<UserGiftingGroupYearVm>
{
    public UserGiftingGroupYearVmValidator()
    {
        // TODO: Prevent changes to 'included' if after the year cutoff
    }
}
