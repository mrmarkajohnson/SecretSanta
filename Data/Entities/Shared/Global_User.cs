using Data.Entities.Santa;
using Global.Abstractions.Global;
using Global.Validation;
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
    
    [Required, Display(Name = "First Name")]
    [MaxLength(UserVal.Forename.MaxLength)]
    public required string Forename { get; set; }

    [Display(Name = "Middle Names"), MaxLength(UserVal.MiddleNames.MaxLength)]
    public string? MiddleNames { get; set; }

    [Required, MaxLength(UserVal.Surname.MaxLength)]
    public required string Surname { get; set; }

    public virtual Santa_User? SantaUser { get; set; }

    public DateTime DateCreated { get; set; }

    [MaxLength(UserVal.SecurityQuestions.MaxLength)]
    public string? SecurityQuestion1 { get; set; }

    public string? SecurityAnswer1 { get; set; }

    [MaxLength(UserVal.SecurityHints.MaxLength)]
    public string? SecurityHint1 { get; set; }

    [MaxLength(UserVal.SecurityQuestions.MaxLength)]
    public string? SecurityQuestion2 { get; set; }

    public string? SecurityAnswer2 { get; set; }

    [MaxLength(UserVal.SecurityHints.MaxLength)]
    public string? SecurityHint2 { get; set; }

    public required string Greeting { get; set; }

    public bool SecurityQuestionsSet => !string.IsNullOrWhiteSpace(SecurityAnswer1) && !string.IsNullOrWhiteSpace(SecurityAnswer2);

    [NotMapped]
    public bool IdentificationHashed { get; set; } = true;
}
