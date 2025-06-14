using Application.Areas.Suggestions.BaseModels;
using FluentValidation;
using Global.Validation;
using System.ComponentModel.DataAnnotations;
using ViewModels.Abstractions;
using static Global.Settings.GlobalSettings;
using static Global.Settings.SuggestionSettings;

namespace ViewModels.Models.Suggestions;

public class ManageSuggestionVm : ManageSuggestion, IFormVm, IModalVm
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

    public bool IsModal { get; set; }
    public bool ShowSaveButton => true;
    public string GroupWidth => IsModal ? ModalGroupWidth : StandardGroupWidth;
}

public class ManageSuggestionVmValidator : AbstractValidator<ManageSuggestionVm>
{
    public ManageSuggestionVmValidator()
    {
        RuleFor(x => x.SuggestionText).NotEmpty().MaximumLength(SuggestionVal.Suggestion.MaxLength);

        RuleFor(x => x.PrioritySetter).NotNullOrEmpty().GreaterThan(0).LessThanOrEqualTo(PriorityLimit);
    }
}

