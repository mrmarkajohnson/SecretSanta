using Global.Abstractions.Areas.GiftingGroup;
using System.ComponentModel.DataAnnotations;

namespace Application.Areas.GiftingGroup.BaseModels;

public class UserGiftingGroupYear : GiftingGroupYearBase, IUserGiftingGroupYear
{   
    public bool GroupAdmin { get; set; }

    [Display(Name = "Participating")]
    public bool Included { get; set; }

    public IUserNamesBase? Recipient { get; set; }

    public string LimitString => Limit > 0 ? $"{CurrencySymbol}{Limit}" : "Not set yet";

    public string RecipientString => Recipient == null
        ? "Not yet selected"
        : $"{Recipient.UserDisplayName} ({Recipient.UserName})";
}
