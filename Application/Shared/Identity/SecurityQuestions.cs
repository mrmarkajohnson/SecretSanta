using Global.Abstractions.Global;
using System.ComponentModel.DataAnnotations;

namespace Application.Shared.Identity;

public class SecurityQuestions : ISecurityQuestions
{
    [Display(Name = "Question")]
    public string? SecurityQuestion1 { get; set; }

    [Display(Name = "Answer")]
    public string? SecurityAnswer1 { get; set; }

    [Display(Name = "Hint")]
    public string? SecurityHint1 { get; set; }

    [Display(Name = "Question")]
    public string? SecurityQuestion2 { get; set; }

    [Display(Name = "Answer")]
    public string? SecurityAnswer2 { get; set; }

    [Display(Name = "Hint")]
    public string? SecurityHint2 { get; set; }
}
