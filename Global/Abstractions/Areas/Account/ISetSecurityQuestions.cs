using FluentValidation;

namespace Global.Abstractions.Areas.Account;

public interface ISetSecurityQuestions : ISecurityQuestions, IConfirmCurrentPassword
{
    bool Update { get; set; }
}

public class SetSecurityQuestionsValidator<TItem> : SecurityQuestionsValidator<TItem> where TItem : ISetSecurityQuestions
{
    public SetSecurityQuestionsValidator()
    {
        RuleFor(x => x.CurrentPassword)
            .NotEmpty()
            .When(x => x.Update);
    }
}