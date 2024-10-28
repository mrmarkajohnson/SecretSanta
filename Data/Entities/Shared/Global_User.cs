using Data.Attributes;
using Data.Entities.Santa;
using Global.Abstractions.Global;
using Global.Abstractions.Global.Account;
using Global.Validation;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities.Shared;

[Table("Global_User")]
public class Global_User : IdentityUser, IEntity, IGlobalUser, ISecurityQuestions, 
    IAuditableEntity<Global_User_Audit, Global_User_AuditChange>
{
    public Global_User()
    {
        DateCreated = DateTime.Now;
        AuditTrail = new HashSet<Global_User_Audit>();
    }
    
    [Required, Audit("First Name")]
    [MaxLength(UserVal.Forename.MaxLength)]
    public required string Forename { get; set; }

    [Audit("Middle Names"), MaxLength(UserVal.MiddleNames.MaxLength)]
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

    [NotAudited]
    public bool SecurityQuestionsSet => !string.IsNullOrWhiteSpace(SecurityAnswer1) && !string.IsNullOrWhiteSpace(SecurityAnswer2);

    [NotMapped, NotAudited]
    public bool IdentificationHashed { get; set; } = true;

    [Audit(NoDetails = true)]
    public override string? PasswordHash { get => base.PasswordHash; set => base.PasswordHash = value; }

    public virtual ICollection<Global_User_Audit> AuditTrail { get; set; }

    public void AddAuditEntry(IAuditBase auditTrail, IList<IAuditBaseChange> changes)
    {
        this.AddNewAuditEntry<Global_User, Global_User_Audit, Global_User_AuditChange>(auditTrail, changes);
    }
}
