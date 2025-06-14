using Application.Shared.BaseModels;
using Global.Abstractions.Areas.Participate;
using System.ComponentModel.DataAnnotations;
using static Global.Settings.GiftingGroupSettings;

namespace Application.Areas.GiftingGroup.BaseModels;

public class UserGiftingGroupYear : GiftingGroupYearBase, IUserGiftingGroupYear
{
    [Display(Name = GiftingGroupNames.MemberStatus)]
    public GroupMemberStatus MemberStatus { get; set; }

    [Display(Name = "Participating")]
    public bool Included { get; set; }

    public UserNamesBase? Recipient { get; set; }
    IUserNamesBase? IUserGiftingGroupYear.Recipient => Recipient;

    public string LimitString => MemberStatus <= GroupMemberStatus.Rejected 
        ? "N/A"
        : (Limit > 0 ? $"{CurrencySymbol}{Limit}" : "Not set yet");

    public string RecipientString => MemberStatus <= GroupMemberStatus.Rejected
        ? "N/A"
        : (Recipient == null 
            ? "Not yet selected"
            : $"{Recipient.UserDisplayName} ({Recipient.UserName})");
}
