using Data.Attributes;
using Data.Entities.Santa;
using Global.Abstractions.Areas.Account;
using Global.Abstractions.Shared;
using Global.Validation;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using static Global.Settings.GlobalSettings;
using static Global.Settings.IdentitySettings;

namespace Data.Entities.Shared;

/// <summary>
/// Allows expansion using the same database and users
/// </summary>
[Table("Global_User")]
public class Global_User : IdentityUser, IEntity, IGlobalUser, ISecurityQuestions,
    IAuditableEntity<Global_User_Audit, Global_User_AuditChange>
{
    public Global_User()
    {
        DateCreated = DateTime.Now;
        AuditTrail = new HashSet<Global_User_Audit>();
    }

    [Required, Audit(UserDisplayNames.Forename)]
    [MaxLength(UserVal.Forename.MaxLength)]
    public required string Forename { get; set; }

    [Audit("Middle Names"), MaxLength(UserVal.MiddleNames.MaxLength)]
    public string? MiddleNames { get; set; }

    [Audit(UserDisplayNames.PreferredNameType)]
    public PreferredNameOption PreferredNameType { get; set; }

    [Audit(UserDisplayNames.PreferredFirstName), MaxLength(UserVal.PreferredFirstName.MaxLength)]
    public string? PreferredFirstName { get; set; }

    [Required, MaxLength(UserVal.Surname.MaxLength)]
    public required string Surname { get; set; }

    public Gender Gender { get; set; }

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
    public bool SecurityQuestionsSet => SecurityAnswer1.IsNotEmpty() && SecurityAnswer2.IsNotEmpty();

    [Audit(NoDetails = true)]
    public override string? PasswordHash { get => base.PasswordHash; set => base.PasswordHash = value; }

    public bool SystemAdmin { get; set; }

    public virtual ICollection<Global_User_Audit> AuditTrail { get; set; }

    #region For the IGlobalUser interface

    [NotMapped, NotAudited]
    public bool IdentificationHashed // can't use explicit implemention as it doesn't map correctly, and it must have a setter
    {
        get => true;
        set
        {
            throw new NotImplementedException("The entity cannot be unhashed."); // just in case!
        }
    }

    [NotMapped, NotAudited]
    public string GlobalUserId => Id; // ditto

    [NotMapped, NotAudited]
    bool IHasEmail.ShowEmail
    {
        get => false;
        set { }
    }

    #endregion For the IGlobalUser interface

    public void AddAuditEntry(IAuditBase auditTrail, IList<IAuditBaseChange> changes)
    {
        this.AddNewAuditEntry<Global_User, Global_User_Audit, Global_User_AuditChange>(auditTrail, changes);
    }

    [NotMapped, NotAudited]
    string IUserAllNames.UserDisplayName => this.DisplayName(false);
}
