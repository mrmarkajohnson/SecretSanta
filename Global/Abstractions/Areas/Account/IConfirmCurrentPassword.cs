using System.ComponentModel.DataAnnotations;

namespace Global.Abstractions.Areas.Account;

public interface IConfirmCurrentPassword : ICheckLockout
{
    [Required]
    [Display(Name = "Current Password"), DataType(DataType.Password)]
    string CurrentPassword { get; set; }
}
