using System.ComponentModel.DataAnnotations;

namespace Global.Abstractions.Global.Account;

public interface IConfirmCurrentPassword
{
    [Required]
    [Display(Name = "Current Password"), DataType(DataType.Password)]
    string CurrentPassword { get; set; }

    string CurrentPasswordLabel { get; }
}
