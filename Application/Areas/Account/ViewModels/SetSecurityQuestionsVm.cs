using Application.Shared.Identity;
using Global.Abstractions.Areas.Account;
using Global.Abstractions.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace Application.Areas.Account.ViewModels;

public sealed class SetSecurityQuestionsVm : SecurityQuestions, IFormVm, ISetSecurityQuestions
{
    public required List<string> Greetings { get; set; }

    [Required]
    [Display(Name = "Password"), DataType(DataType.Password)]
    public string CurrentPassword { get; set; } = string.Empty;

    public bool Update { get; set; }
    public bool LockedOut { get; set; }

    public string? ReturnUrl { get; set; }
    public string? SuccessMessage { get; set; }
    public string SubmitButtonText { get; set; } = "Save";
    public string SubmitButtonIcon { get; set; } = "fa-save";

    public string? InvitationWaitMessage { get; set; }
}

public sealed class SetSecurityQuestionsVmValidator : SetSecurityQuestionsValidator<SetSecurityQuestionsVm>
{
}
