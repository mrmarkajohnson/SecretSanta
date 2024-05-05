using Global.Abstractions.Global;
using System.ComponentModel.DataAnnotations;

namespace Application.Shared.Identity;

public class GlobalUser : CoreIdentityUser, IGlobalUser
{
    [Display(Name = "First Name")]
    public required string Forename { get; set; }

    [Display(Name = "Middle Names")]
    public string? MiddleNames { get; set; }

    public required string Surname { get; set; }

    public bool SecurityQuestionsSet { get; set; }
}
