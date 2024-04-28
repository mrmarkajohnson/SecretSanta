using System.ComponentModel.DataAnnotations;

namespace ViewLayer.Models.Account;

public class LoginVm
{
    //public IList<AuthenticationScheme> ExternalLogins { get; set; }

    public string? ReturnUrl { get; set; }
    
    [Display(Name = "E-mail or Username")]
    public required string EmailOrUserName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public required string Password { get; set; }

    [Display(Name = "Remember me")]
    public bool RememberMe { get; set; }

}
