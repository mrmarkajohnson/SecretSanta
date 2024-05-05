using Data.Entities.Santa;
using Global.Abstractions.Global;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities.Shared;

[Table("Global_User")]
public class Global_User : IdentityUser, IEntity, IGlobalUser, ISecurityQuestions
{
    public Global_User()
    {
        DateCreated = DateTime.Now;
    }
    
    [Required, Length(2, 250)]
    public required string Forename { get; set; }

    [Display(Name = "Middle Names")]
    public string? MiddleNames { get; set; }

    [Required, Length(2, 250)]
    public required string Surname { get; set; }

    public virtual Santa_User? SantaUser { get; set; }

    public DateTime DateCreated { get; set; }

    public string? SecurityQuestion1 { get; set; }
    public string? SecurityAnswer1 { get; set; }
    public string? SecurityHint1 { get; set; }

    public string? SecurityQuestion2 { get; set; }
    public string? SecurityAnswer2 { get; set; }
    public string? SecurityHint2 { get; set; }

    public bool SecurityQuestionsSet => !string.IsNullOrWhiteSpace(SecurityAnswer1) && !string.IsNullOrWhiteSpace(SecurityAnswer2);
}
