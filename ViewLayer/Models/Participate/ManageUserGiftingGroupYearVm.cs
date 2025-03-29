using Application.Areas.GiftingGroup.BaseModels;
using FluentValidation;
using Global.Abstractions.Areas.GiftingGroup;
using Global.Names;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ViewLayer.Models.Participate;

public class ManageUserGiftingGroupYearVm : ManageUserGiftingGroupYear, IManageUserGiftingGroupYear, IForm
{
    public bool CanChangeIncluded => Recipient == null; // TODO: Prevent changes to 'included' if after the year cutoff

    public IList<SelectListItem> OtherMembersSelect => OtherGroupMembers
        .Select(x => new SelectListItem { Value = x.GlobalUserId, Text = x.UserDisplayName })
        .ToList();

    public bool IncludePreviousYears { get; set; }
    public bool SubmitIncludedChangeImmediately => !IncludePreviousYears;

    public string? ReturnUrl { get; set; }
    public string SubmitButtonText { get; set; } = "Save";
    public string SubmitButtonIcon { get; set; } = "fa-save";
    public string? SuccessMessage { get; set; }
}

public class UserGiftingGroupYearVmValidator : AbstractValidator<ManageUserGiftingGroupYearVm>
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
