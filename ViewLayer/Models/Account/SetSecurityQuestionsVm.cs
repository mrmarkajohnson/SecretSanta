using Application.Shared.Identity;
using Global.Abstractions.Global.Account;
using System.ComponentModel.DataAnnotations;

namespace ViewLayer.Models.Account;

public class SetSecurityQuestionsVm : SecurityQuestions, IForm, ISetSecurityQuestions
{
    public required List<string> Greetings { get; set; }
    
    public string? ReturnUrl { get; set; }
    public string? SuccessMessage { get; set; }

    public string SubmitButtonText { get; set; } = "Save";
    public string SubmitButtonIcon { get; set; } = "fa-save";

    [Required]
    [Display(Name = "Password"), DataType(DataType.Password)]
    public string CurrentPassword { get; set; } = "";

    public bool Update { get; set; }
    public bool LockedOut { get; set; }
}

public class SetSecurityQuestionsVmValidator : SetSecurityQuestionsValidator<SetSecurityQuestionsVm>
{
}
