using Application.Areas.Suggestions.BaseModels;
using FluentValidation;
using Global.Validation;
using ViewLayer.Abstractions;
using static Global.Settings.SuggestionSettings;

namespace ViewLayer.Models.Suggestions;

public class ManageSuggestionVm : ManageSuggestion, IFormVm
{
    public int? PrioritySetter
    {
        get => Priority <= 0 || Priority > PriorityLimit ? null : Priority;
        set => Priority = value ?? 0;
    }
    
    public string? ReturnUrl { get; set; }
    public string SubmitButtonText { get; set; } = "Save";
    public string SubmitButtonIcon { get; set; } = "fa-save";
    public string? SuccessMessage { get; set; }
}

public class ManageSuggestionVmValidator : AbstractValidator<ManageSuggestionVm>
{
    public ManageSuggestionVmValidator()
    {
        RuleFor(x => x.SuggestionText).NotEmpty().MaximumLength(SuggestionVal.Suggestion.MaxLength);

        RuleFor(x => x.PrioritySetter).NotNullOrEmpty().GreaterThan(0).LessThanOrEqualTo(PriorityLimit);
    }
}

