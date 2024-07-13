using Application.Shared.Identity;
using Global.Abstractions.Global.Account;

namespace ViewLayer.Models.Account;

public class SetSecurityQuestionsVm : SecurityQuestions, IForm, ISecurityQuestions
{
    public required List<string> Greetings { get; set; }
    
    public string? ReturnUrl { get; set; }
    public string? SuccessMessage { get; set; }

    public string SubmitButtonText { get; set; } = "Save";
    public string SubmitButtonIcon { get; set; } = "fa-save";

    public bool Update { get; set; }
}

public class SetSecurityQuestionsVmValidator : SecurityQuestionsValidator<SetSecurityQuestionsVm>
{
}
