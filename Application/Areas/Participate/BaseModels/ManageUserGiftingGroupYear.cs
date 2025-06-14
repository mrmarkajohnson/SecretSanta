using Global.Abstractions.Areas.Participate;
using System.ComponentModel.DataAnnotations;

namespace Application.Areas.Participate.BaseModels;

public class ManageUserGiftingGroupYear : UserGiftingGroupYear, IManageUserGiftingGroupYear
{
    public ManageUserGiftingGroupYear()
    {
        OtherGroupMembers = new List<IUserNamesBase>();
    }

    public int PreviousYearsRequired { get; set; }

    [Display(Name = "Who did you give to last year?")]
    public string? LastRecipientUserId { get; set; }

    [Display(Name = "Who did you give to the year before?")]
    public string? PreviousRecipientUserId { get; set; }

    public IList<IUserNamesBase> OtherGroupMembers { get; set; }
}
