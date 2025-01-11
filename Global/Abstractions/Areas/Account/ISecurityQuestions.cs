using FluentValidation;
using Global.Settings;
using Global.Validation;

namespace Global.Abstractions.Areas.Account;

public interface ISecurityQuestions
{
    string? SecurityQuestion1 { get; set; }
    string? SecurityAnswer1 { get; set; }
    string? SecurityHint1 { get; set; }

    string? SecurityQuestion2 { get; set; }
    string? SecurityAnswer2 { get; set; }
    string? SecurityHint2 { get; set; }

    string Greeting { get; set; }

    bool SecurityQuestionsSet { get; }
}

public class SecurityQuestionsValidator<TItem> : AbstractValidator<TItem> where TItem : ISecurityQuestions
{
    public SecurityQuestionsValidator()
    {
        RuleFor(x => x.Greeting).Must(x => Greetings.Messages.Any(y => Equals(y, x))).WithMessage("Greeting not found. Please click 'Change' to select another.");

        RuleFor(x => x.SecurityQuestion1).NotNull().NotEmpty().Length(UserVal.SecurityQuestions.MinLength, UserVal.SecurityQuestions.MaxLength);
        RuleFor(x => x.SecurityAnswer1).NotNull().NotEmpty().Length(UserVal.SecurityAnswers.MinLength, UserVal.SecurityAnswers.MaxLength);
        RuleFor(x => x.SecurityHint1).MaximumLength(UserVal.SecurityHints.MaxLength);

        RuleFor(x => x.SecurityQuestion2).NotNull().NotEmpty().Length(UserVal.SecurityQuestions.MinLength, UserVal.SecurityQuestions.MaxLength);
        RuleFor(x => x.SecurityAnswer2).NotNull().NotEmpty().Length(UserVal.SecurityAnswers.MinLength, UserVal.SecurityAnswers.MaxLength);
        RuleFor(x => x.SecurityHint2).MaximumLength(UserVal.SecurityHints.MaxLength);

        RuleFor(x => x.SecurityQuestion2).NotEqual(x => x.SecurityQuestion1).WithMessage("Please enter two different questions.");
    }
}
