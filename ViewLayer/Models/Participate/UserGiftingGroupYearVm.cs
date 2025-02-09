using Application.Areas.GiftingGroup.BaseModels;
using FluentValidation;
using Global.Abstractions.Areas.GiftingGroup;
using Global.Names;

namespace ViewLayer.Models.Participate;

public class UserGiftingGroupYearVm : UserGiftingGroupYear, IUserGiftingGroupYear, IForm
{
    public bool CanChangeIncluded => Recipient == null; // TODO: Prevent changes to 'included' if after the year cutoff

    public string? ReturnUrl { get; set; }
    public string SubmitButtonText { get; set; } = "Save";
    public string SubmitButtonIcon { get; set; } = "fa-save";
    public string? SuccessMessage { get; set; }
}

public class UserGiftingGroupYearVmValidator : AbstractValidator<UserGiftingGroupYearVm>
{
    public UserGiftingGroupYearVmValidator()
    {
        RuleFor(x => x.Included)
            .NotEqual(false)
            .When(x => x.Recipient != null)
            .WithMessage(GiftingGroupNames.NoOptOutWithRecipient);
        
        // TODO: Prevent changes to 'included' if after the year cutoff
    }
}
