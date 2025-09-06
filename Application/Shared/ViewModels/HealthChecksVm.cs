using Global.Abstractions.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace Application.Shared.ViewModels;

public class HealthChecksVm : IFormVm
{
    [Display(Name = "Database Server")]
    public string? Server { get; set; }

    [Display(Name="Database UserID")]
    public string? SafeDatabaseUserID { get; set; }

    [Display(Name = "Database Password")]
    public string? SafeDatabasePassword { get; set; }

    [Display(Name = "Symmetric Key End")]
    public string? SafeKeyEnd { get; set; }

    [Display(Name = "Default Connection String")]
    public string? DefaultConnection { get; set; }

    [Display(Name = "Database Connection String")]
    public string? SafeConnectionString { get; set; }

    [Display(Name = "E-mail UserID")]
    public string? SafeMailUserID { get; set; }

    [Display(Name = "E-mail Password")]
    public string? SafeMailPassword { get; set; }

    [Display(Name = "E-mail FromAddress")]
    public string? SafeMailFrom { get; set; }

    [Display(Name = "E-mail SMTP Host")]
    public string? SmtpHost { get; set; }

    [Display(Name = "E-mail SMTP Port")]
    public int? SmtpPort { get; set; }

    [Display(Name = "E-mail Test Address")]
    public string? SafeTestMailAddress { get; set; }

    [Display(Name = "Query Result")]
    public string? QueryResult { get; set; }

    [Display(Name = "Post Result")]
    public string? PostResult { get; set; }

    public string? ReturnUrl { get; set; }
    public string SubmitButtonText { get; set; } = "Test Post";
    public string SubmitButtonIcon { get; set; } = "fa-flask-vial";
    public string? SuccessMessage { get; set; }
}