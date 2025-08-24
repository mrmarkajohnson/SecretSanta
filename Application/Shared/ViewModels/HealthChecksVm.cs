using Global.Abstractions.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace Application.Shared.ViewModels;

public class HealthChecksVm : IFormVm
{
    public string? Server { get; set; }

    [Display(Name="Database UserID")]
    public string? SafeUserID { get; set; }

    [Display(Name = "Database Password")]
    public string? SafePassword { get; set; }

    [Display(Name = "Symmetric Key End")]
    public string? SafeKeyEnd { get; set; }

    [Display(Name = "Default Connection String")]
    public string? DefaultConnection { get; set; }

    [Display(Name = "Database Connection String")]
    public string? SafeConnectionString { get; set; }

    [Display(Name = "Query Result")]
    public string? QueryResult { get; set; }

    [Display(Name = "Post Result")]
    public string? PostResult { get; set; }

    public string? ReturnUrl { get; set; }
    public string SubmitButtonText { get; set; } = "Test Post";
    public string SubmitButtonIcon { get; set; } = "fa-flask-vial";
    public string? SuccessMessage { get; set; }
}