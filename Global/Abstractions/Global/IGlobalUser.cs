using System.ComponentModel.DataAnnotations;

namespace Global.Abstractions.Global;

public interface IGlobalUser
{
    [Required, Length(2, 250), Display(Name = "Forename(s)")]
    string Forename { get; set; }

    [Display(Name = "Middle Names")]
    string? MiddleNames { get; set; }

    [Required, Length(2, 250), Display(Name = "Surname")]
    public string Surname { get; set; }
}
