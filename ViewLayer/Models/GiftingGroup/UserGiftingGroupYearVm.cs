using Application.Areas.GiftingGroup.BaseModels;
using FluentValidation;
using Global.Abstractions.Areas.GiftingGroup;

namespace ViewLayer.Models.GiftingGroup;

public class UserGiftingGroupYearVm : UserGiftingGroupYear, IUserGiftingGroupYear, IForm
{
    public string LimitString => Limit > 0 ? $"{CurrencySymbol}{Limit}" : "Not set yet";

    public string RecipientString => Recipient == null 
        ? "Not calculated yet" 
        : $"{Recipient.UserDisplayName} ({Recipient.UserName})";

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
