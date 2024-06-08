using System.ComponentModel.DataAnnotations;

namespace Global.Abstractions.Santa.Areas.Account;

public interface IRegisterSantaUser : ISantaUser
{
    [DataType(DataType.Password)]
    string Password { get; set; }

    string ConfirmPassword { get; set; }
}
