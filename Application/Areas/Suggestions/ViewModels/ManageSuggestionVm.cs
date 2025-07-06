using Application.Areas.Suggestions.BaseModels;
using FluentValidation;
using Global.Abstractions.ViewModels;
using System.ComponentModel.DataAnnotations;
using static Global.Settings.GlobalSettings;
using static Global.Settings.SuggestionSettings;

namespace Application.Areas.Suggestions.ViewModels;

public class ManageSuggestionVm : ManageSuggestion, IOptionalModalFormVm
{
    [Display(Name = "Priority")]
    public int? PrioritySetter
    {
        get => Priority <= 0 || Priority > PriorityLimit ? null : Priority;
        set => Priority = value ?? 0;
    }

    public string? ReturnUrl { get; set; }
    public string SubmitButtonText { get; set; } = "Save";
    public string SubmitButtonIcon { get; set; } = "fa-save";
    public string? SuccessMessage { get; set; }

    public bool Existing => SuggestionKey > 0;

    public string PageTitle => Existing ? "Edit Suggestion" : "Add Suggestion";
    public string ModalTitle => PageTitle;
    public string SubTitle => Existing ? "Update your suggestion details" : "Suggest presents for Santa";
    public List<string> Guidance => GetGuidance();

    public bool IsModal { get; set; }
    public bool ShowSaveButton => true;
    public string? AdditionalFooterButtonPartial { get; }
    public string GroupWidth => IsModal ? ModalGroupWidth : StandardGroupWidth;

    private List<string> GetGuidance()
    {
        List<string> guidance = Existing ? [] : ["Suggest a present that you'd like to receive from your Secret Santa."];

        if (YearGroupUserLinks.Count > 0)
        {
            guidance.Add("You can assign your suggestion to one or more groups. Make sure it is realistic for the spending limit of each group.");
        }
        else if (YearGroupUserLinks.Count == 1)
        {
            guidance.Add($"Make sure your suggestion is realistic for the spending limit of the '{YearGroupUserLinks[0].GiftingGroupName}' group.");
        }

        return guidance;
    }
}

public class ManageSuggestionVmValidator : AbstractValidator<ManageSuggestionVm>
{
    public ManageSuggestionVmValidator()
    {
        RuleFor(x => x.SuggestionText).NotEmpty().MaximumLength(SuggestionVal.Suggestion.MaxLength);

        RuleFor(x => x.PrioritySetter).NotNullOrEmpty().GreaterThan(0).LessThanOrEqualTo(PriorityLimit);
    }
}

