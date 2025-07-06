using Application.Shared.ViewModels;
using Global.Abstractions.Areas.GiftingGroup;
using Global.Abstractions.Areas.Messages;
using System.ComponentModel.DataAnnotations;
using static Global.Settings.MessageSettings;

namespace Application.Areas.Messages.ViewModels;

public class ChooseMessageRecipientVm : BaseFormVm, IForm, IChooseMessageRecipient
{
    public ChooseMessageRecipientVm()
    {
        GiftingGroups = new List<IUserGiftingGroup>();
        OtherGroupMembers = new List<IGroupMember>();
    }

    public int? ReplyToMessageKey { get; set; }
    public string? ReplyToName { get; set; }
    public MessageRecipientType? OriginalRecipientType { get; set; }
    public virtual bool IsReply => ReplyToMessageKey > 0;

    [Display(Name = "Include Future Members")]
    public bool IncludeFutureMembers { get; set; }

    [Display(Name = "To")]
    public virtual MessageRecipientType RecipientType { get; set; } = MessageRecipientType.TBC;

    [Display(Name = "To")]
    public int? SpecificGroupMemberKey { get; set; }

    [Display(Name = "For Group")]
    public int? GiftingGroupKey { get; set; }

    public virtual string? GroupName
    {
        get => GiftingGroupKey > 0
            ? GiftingGroups.FirstOrDefault(x => x.GiftingGroupKey == GiftingGroupKey)?.GroupName
            : null;
        set { } // allows setter override
    }

    public bool GroupAdmin { get; set; }

    public IList<IUserGiftingGroup> GiftingGroups { get; set; }
    public virtual IList<StandardSelectable> GroupSelection => GetSelectableGroups();

    public virtual List<MessageRecipientType> AvailableRecipientTypes => IsReply
        ? GetReplyRecipientTypes()
        : GetOriginalRecipientTypes();

    public IList<IGroupMember> OtherGroupMembers { get; set; }
    public virtual IList<StandardSelectable> MemberSelection => GetSelectableMembers();

    protected List<MessageRecipientType> GetReplyRecipientTypes()
    {
        return OriginalRecipientType switch
        {
            MessageRecipientType.GiftRecipient or MessageRecipientType.Gifter or MessageRecipientType.OriginalSender
                or MessageRecipientType.PotentialPartner or MessageRecipientType.SingleGroupMember
                    => [MessageRecipientType.OriginalSender],
            _ => ReplyRecipientTypes
        };
    }

    private List<MessageRecipientType> GetOriginalRecipientTypes()
    {
        return OriginalRecipientTypes
            .Where(x => x != MessageRecipientType.GroupAdmins || !GroupAdmin || OtherGroupMembers.Any(y => y.GroupAdmin))
            .Where(x => !x.SpecificMember() || MemberSelection.Count > 0)
            .ToList();
    }

    private List<StandardSelectable> GetSelectableGroups()
    {
        return GiftingGroups.Select(x => new StandardSelectable(x.GiftingGroupKey, x.GroupName)).ToList();
    }

    private List<StandardSelectable> GetSelectableMembers()
    {
        return OtherGroupMembers.Select(x => new StandardSelectable(x.SantaUserKey, x.UserDisplayName)).ToList();
    }
}
