using Global.Validation;
using System.ComponentModel.DataAnnotations;

namespace Application.Shared.Identity;

public class SecurityQuestions : ISecurityQuestions
{
    [Display(Name = "Question"), StringLength(UserDetails.SecurityQuestions.MaxLength,
        ErrorMessage = "Questions must be {2} to {1} characters long.", MinimumLength = UserDetails.SecurityQuestions.MinLength)]
    public string? SecurityQuestion1 { get; set; }

    [Display(Name = "Answer"), StringLength(UserDetails.SecurityAnswers.MaxLength,
        ErrorMessage = "Answers must be {2} to {1} characters long.", MinimumLength = UserDetails.SecurityAnswers.MinLength)]
    public string? SecurityAnswer1 { get; set; }

    [Display(Name = "Hint"), MaxLength(UserDetails.SecurityHints.MaxLength)]
    public string? SecurityHint1 { get; set; }

    [Display(Name = "Question"), StringLength(UserDetails.SecurityQuestions.MaxLength,
        ErrorMessage = "Questions must be {2} to {1} characters long.", MinimumLength = UserDetails.SecurityQuestions.MinLength)]
    public string? SecurityQuestion2 { get; set; }

    [Display(Name = "Answer"), StringLength(UserDetails.SecurityHints.MaxLength,
        ErrorMessage = "Answers must be {2} to {1} characters long.", MinimumLength = UserDetails.SecurityAnswers.MinLength)]
    public string? SecurityAnswer2 { get; set; }

    [Display(Name = "Hint"), MaxLength(UserDetails.SecurityHints.MaxLength)]
    public string? SecurityHint2 { get; set; }

    public bool SecurityQuestionsSet => !string.IsNullOrWhiteSpace(SecurityAnswer1) && !string.IsNullOrWhiteSpace(SecurityAnswer2);
}
