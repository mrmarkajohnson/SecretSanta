using Application.Shared.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace Application.Areas.Account.ViewModels;

public class CloseAccountVm : BaseFormVm
{
    [Display(Name="Confirm Closure")]
    public string? Message { get; set; }
}
