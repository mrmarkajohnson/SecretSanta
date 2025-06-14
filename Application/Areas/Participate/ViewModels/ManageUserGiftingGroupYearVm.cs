using Application.Areas.Participate.BaseModels;
using Application.Areas.Suggestions.ViewModels;
using FluentValidation;
using Global.Abstractions.Areas.Participate;
using Global.Abstractions.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Application.Areas.Participate.ViewModels;

public sealed class ManageUserGiftingGroupYearVm : ManageUserGiftingGroupYear, IManageUserGiftingGroupYear, IFormVm
{
    public bool CanChangeIncluded => Recipient == null; // TODO: Prevent changes to 'included' if after the year cutoff

    public IList<SelectListItem> OtherMembersSelect => OtherGroupMembers
        .Select(x => new SelectListItem { Value = x.GlobalUserId, Text = x.UserDisplayName })
        .ToList();

    public RecipientSuggestionsVm RecipientSuggestions { get; set; } = new();

    public bool IncludePreviousYears { get; set; }
    public bool SubmitIncludedChangeImmediately => !IncludePreviousYears;

    public string? ReturnUrl { get; set; }
    public string SubmitButtonText { get; set; } = "Save";
    public string SubmitButtonIcon { get; set; } = "fa-save";
    public string? SuccessMessage { get; set; }
}

public sealed class UserGiftingGroupYearVmValidator : AbstractValidator<ManageUserGiftingGroupYearVm>
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
