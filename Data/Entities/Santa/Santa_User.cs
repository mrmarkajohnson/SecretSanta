using Data.Entities.Shared;
using Global.Abstractions.Shared;
using Global.Settings;

namespace Data.Entities.Santa;

public class Santa_User : DeletableBaseEntity, IDeletableEntity, IEmailPreferences
{
    public Santa_User()
    {
        SuggestedRelationships = new HashSet<Santa_PartnerLink>();
        ConfirmingRelationships = new HashSet<Santa_PartnerLink>();
        GiftingGroupLinks = new HashSet<Santa_GiftingGroupUser>();
        GiftingGroupYears = new HashSet<Santa_YearGroupUser>();
        RecipientYears = new HashSet<Santa_YearGroupUser>();
        GiftingGroupApplications = new HashSet<Santa_GiftingGroupApplication>();
        GiftingGroupApplicationResponses = new HashSet<Santa_GiftingGroupApplication>();
        SentMessages = new HashSet<Santa_Message>();
        ReceivedMessages = new HashSet<Santa_MessageRecipient>();
        Suggestions = new HashSet<Santa_Suggestion>();
        SentInvitations = new HashSet<Santa_Invitation>();
        ReceivedInvitations = new HashSet<Santa_Invitation>();
    }

    [Key]
    public int SantaUserKey { get; set; }

    public MessageSettings.EmailPreference ReceiveEmails { get; set; }
    public bool DetailedEmails { get; set; }

    public required string GlobalUserId { get; set; }
    public virtual required Global_User GlobalUser { get; set; }

    /// <summary>
    /// Relationships suggested by this user
    /// </summary>
    public virtual ICollection<Santa_PartnerLink> SuggestedRelationships { get; set; }

    /// <summary>
    /// Relationships suggested by another user, (to be) confirmed by this user
    /// </summary>
    public virtual ICollection<Santa_PartnerLink> ConfirmingRelationships { get; set; }

    public virtual ICollection<Santa_GiftingGroupUser> GiftingGroupLinks { get; set; }
    public virtual ICollection<Santa_YearGroupUser> GiftingGroupYears { get; set; }
    public virtual ICollection<Santa_YearGroupUser> RecipientYears { get; set; }
    public virtual ICollection<Santa_GiftingGroupApplication> GiftingGroupApplications { get; set; }
    public virtual ICollection<Santa_GiftingGroupApplication> GiftingGroupApplicationResponses { get; set; }

    public virtual ICollection<Santa_Message> SentMessages { get; set; }
    public virtual ICollection<Santa_MessageRecipient> ReceivedMessages { get; set; }
    public virtual ICollection<Santa_Suggestion> Suggestions { get; set; }
    public virtual ICollection<Santa_Invitation> SentInvitations { get; set; }
    public virtual ICollection<Santa_Invitation> ReceivedInvitations { get; set; }

    public IList<string> GroupNames() => GiftingGroupLinks
        .Where(x => x.DateArchived == null && x.DateDeleted == null)
        .Select(x => x.GiftingGroup.Name)
        .ToList();

    public IList<int> UserKeysForVisibleEmail() => SuggestedRelationships
        .Where(x => x.Confirmed == true && x.RelationshipEnded == null && x.DateDeleted == null && x.DateArchived == null)
        .Select(y => y.ConfirmingSantaUserKey)
        .Union(ConfirmingRelationships
            .Where(x => x.RelationshipEnded == null && x.DateDeleted == null && x.DateArchived == null)
            .Select(y => y.SuggestedBySantaUserKey))
        .Union(GiftingGroupLinks
            .Where(x => x.DateArchived == null && x.DateDeleted == null)
            .Where(x => x.GroupAdmin)
            .SelectMany(y => y.GiftingGroup.Members)
            .Select(z => z.SantaUserKey))
        .ToList();
}
