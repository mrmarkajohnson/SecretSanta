using FluentValidation;
using Global.Validation;

namespace Global.Abstractions.Global;

public interface ISecurityQuestions
{
    string? SecurityQuestion1 { get; set; }
    string? SecurityAnswer1 { get; set; }
    string? SecurityHint1 {  get; set; }

    string? SecurityQuestion2 { get; set; }
    string? SecurityAnswer2 { get; set; }
    string? SecurityHint2 { get; set; }

    bool SecurityQuestionsSet { get; }
}

public class SecurityQuestionsValidator<TItem> : AbstractValidator<TItem> where TItem : ISecurityQuestions
{
    public SecurityQuestionsValidator()
    {
        RuleFor(x => x.SecurityQuestion1).NotNull().NotEmpty().Length(UserVal.SecurityQuestions.MinLength, UserVal.SecurityQuestions.MaxLength);
        RuleFor(x => x.SecurityAnswer1).NotNull().NotEmpty().Length(UserVal.SecurityAnswers.MinLength, UserVal.SecurityAnswers.MaxLength);
        RuleFor(x => x.SecurityHint1).MaximumLength(UserVal.SecurityHints.MaxLength);

        RuleFor(x => x.SecurityQuestion2).NotNull().NotEmpty().Length(UserVal.SecurityQuestions.MinLength, UserVal.SecurityQuestions.MaxLength);
        RuleFor(x => x.SecurityAnswer2).NotNull().NotEmpty().Length(UserVal.SecurityAnswers.MinLength, UserVal.SecurityAnswers.MaxLength);
        RuleFor(x => x.SecurityHint2).MaximumLength(UserVal.SecurityHints.MaxLength);

        RuleFor(x => x.SecurityQuestion2).NotEqual(x => x.SecurityQuestion1).WithMessage("Please enter two different questions.");
    }
}
