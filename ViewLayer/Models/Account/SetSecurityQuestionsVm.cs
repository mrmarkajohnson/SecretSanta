using Application.Shared.Identity;
using Global.Abstractions.Global;

namespace ViewLayer.Models.Account;

public class SetSecurityQuestionsVm : SecurityQuestions, IForm, ISecurityQuestions
{
    public string? ReturnUrl { get; set; }
    public string SubmitButtonText { get; set; } = "Save";
    public string SubmitButtonIcon { get; set; } = "fa-save";

    public bool Update { get; set; }
}
