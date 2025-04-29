using Global.Abstractions.Areas.Account;
using System.ComponentModel.DataAnnotations;

namespace Application.Shared.Identity;

public class SecurityQuestions : ISecurityQuestions
{
    [Required, Display(Name = "Question 1"), StringLength(UserVal.SecurityQuestions.MaxLength,
        ErrorMessage = "Questions must be {2} to {1} characters long.", MinimumLength = UserVal.SecurityQuestions.MinLength)]
    public string? SecurityQuestion1 { get; set; }

    [Required, Display(Name = "Answer 1"), StringLength(UserVal.SecurityAnswers.MaxLength,
        ErrorMessage = "Answers must be {2} to {1} characters long.", MinimumLength = UserVal.SecurityAnswers.MinLength)]
    public string? SecurityAnswer1 { get; set; }

    [Display(Name = "Hint 1"), MaxLength(UserVal.SecurityHints.MaxLength)]
    public string? SecurityHint1 { get; set; }

    [Required, Display(Name = "Question 2"), StringLength(UserVal.SecurityQuestions.MaxLength,
        ErrorMessage = "Questions must be {2} to {1} characters long.", MinimumLength = UserVal.SecurityQuestions.MinLength)]
    public string? SecurityQuestion2 { get; set; }

    [Required, Display(Name = "Answer 2"), StringLength(UserVal.SecurityAnswers.MaxLength,
        ErrorMessage = "Answers must be {2} to {1} characters long.", MinimumLength = UserVal.SecurityAnswers.MinLength)]
    public string? SecurityAnswer2 { get; set; }

    [Display(Name = "Hint 2"), MaxLength(UserVal.SecurityHints.MaxLength)]
    public string? SecurityHint2 { get; set; }

    [Display(Name = "Greeting")]
    public required string Greeting { get; set; }

    public bool SecurityQuestionsSet => !string.IsNullOrWhiteSpace(SecurityAnswer1) && !string.IsNullOrWhiteSpace(SecurityAnswer2);
}
