using FluentValidation;

namespace Global.Abstractions.Global;

public interface ISecurityQuestions
{
    string? SecurityQuestion1 { get; set; }
    string? SecurityAnswer1 { get; set; }
    string? SecurityHint1 {  get; set; }

    string? SecurityQuestion2 { get; set; }
    string? SecurityAnswer2 { get; set; }
    string? SecurityHint2 { get; set; }
}

public class SecurityQuestionsValidator : AbstractValidator<ISecurityQuestions>
{
    public SecurityQuestionsValidator()
    {
        RuleFor(x => x.SecurityQuestion1).NotEmpty().Length(10, 500);
        RuleFor(x => x.SecurityAnswer1).NotEmpty().Length(10, 500);
        RuleFor(x => x.SecurityHint1).MaximumLength(500);
        RuleFor(x => x.SecurityQuestion1).NotEmpty().Length(10, 500);
        RuleFor(x => x.SecurityAnswer2).NotEmpty().Length(10, 500);
        RuleFor(x => x.SecurityHint2).MaximumLength(500);
    }
}
