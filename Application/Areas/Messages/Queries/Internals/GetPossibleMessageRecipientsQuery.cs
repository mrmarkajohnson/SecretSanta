using Application.Shared.Requests;
using Global.Abstractions.Areas.Messages;
using Global.Extensions.Exceptions;
using static Global.Settings.MessageSettings;

namespace Application.Areas.Messages.Queries.Internals;

public class GetPossibleMessageRecipientsQuery : BaseQuery<IList<Santa_User>>
{
    private readonly Santa_User _dbSender;
    private readonly Santa_GiftingGroupYear _dbGiftingGroupYear;
    private readonly int? _replyToMessageKey;
    private readonly MessageRecipientType _recipientType;
    private readonly int? _specificGroupMemberKey;

    public GetPossibleMessageRecipientsQuery(Santa_User dbSender, Santa_GiftingGroupYear dbGiftingGroupYear,
        int? replyToMessageKey, MessageRecipientType recipientType, int? specificGroupMemberKey)
    {
        _dbSender = dbSender;
        _dbGiftingGroupYear = dbGiftingGroupYear;
        _replyToMessageKey = replyToMessageKey;
        _recipientType = recipientType;
        _specificGroupMemberKey = specificGroupMemberKey;
    }

    public GetPossibleMessageRecipientsQuery(Santa_User dbSender, Santa_GiftingGroupYear dbGiftingGroupYear,
        IWriteSantaMessage message)
    {
        _dbSender = dbSender;
        _dbGiftingGroupYear = dbGiftingGroupYear;
        _replyToMessageKey = message.ReplyToMessageKey;
        _recipientType = message.RecipientType;
        _specificGroupMemberKey = message.SpecificGroupMemberKey;
    }

    protected override Task<IList<Santa_User>> Handle()
    {
        IList<Santa_User> dbRecipients = GetPossibleRecipients(_dbSender, _replyToMessageKey, 
            _recipientType, _specificGroupMemberKey, true);

        return Task.FromResult(dbRecipients);
    }

    private IList<Santa_User> GetPossibleRecipients(Santa_User dbSender, int? replyToMessageKey, 
        MessageRecipientType recipientType, int? specificGroupMemberKey, bool checkReplySecurity)
    {
        IEnumerable<Santa_User> dbPossibleRecipients = recipientType switch // may include the current user, who is removed below
        {
            MessageRecipientType.SystemAdmins => [], // TODO: Add System Admins once they exist
            MessageRecipientType.GroupAdmins 
                => GetGroupAdmins(dbSender),
            MessageRecipientType.GiftRecipient 
                => GetRecipient(dbSender),
            MessageRecipientType.Gifter 
                => GetGifter(dbSender),
            MessageRecipientType.YearGroupCurrentMembers or MessageRecipientType.YearGroupAllEverMembers
                => GetParticipatingSantaUsers(),
            MessageRecipientType.GroupCurrentMembers or MessageRecipientType.GroupAllEverMembers
                => GetGroupMembers(dbSender),
            MessageRecipientType.OriginalSender
                => GetOriginalSender(dbSender, replyToMessageKey, checkReplySecurity),
            MessageRecipientType.OriginalCurrentRecipients or MessageRecipientType.OriginalAllEverRecipients
                => GetOriginalRecipients(dbSender, replyToMessageKey, checkReplySecurity),
            MessageRecipientType.PotentialPartner
                => [], // should be handled elsewhere
            MessageRecipientType.SingleGroupMember
                => GetSpecificGroupMember(dbSender, specificGroupMemberKey),
            _ => []
        };

        IList<Santa_User> dbRecipients = dbPossibleRecipients
            .Where(x => x.SantaUserKey != dbSender.SantaUserKey)
            .ToList();
        return dbRecipients;
    }

    private IList<Santa_User> GetPossibleRecipients(Santa_Message dbMessage)
    {
        return GetPossibleRecipients(dbMessage.Sender, dbMessage.ReplyTo?.OriginalMessageKey,
            dbMessage.RecipientType, dbMessage.Recipients.FirstOrDefault()?.RecipientSantaUserKey, false);
    }

    private IEnumerable<Santa_User> GetGroupAdmins(Santa_User dbSender)
    {
        return _dbGiftingGroupYear.GiftingGroup
            .OtherMembers(dbSender)
            .Where(x => x.GroupAdmin)
            .Select(x => x.SantaUser);
    }

    private IEnumerable<Santa_User> GetRecipient(Santa_User dbSender)
    {
        return GetParticipants()
            .Where(x => x.SantaUserKey == dbSender.SantaUserKey)
            .Where(x => x.RecipientSantaUser != null)
            .Select(x => x.RecipientSantaUser);
    }

    private IEnumerable<Santa_User> GetGifter(Santa_User dbSender)
    {
        return GetParticipants()
            .Where(x => x.RecipientSantaUserKey == dbSender.SantaUserKey)
            .Select(x => x.SantaUser);
    }

    private IEnumerable<Santa_User> GetParticipatingSantaUsers()
    {
        return GetParticipants().Select(x => x.SantaUser);
    }

    private List<Santa_YearGroupUser> GetParticipants()
    {
        return _dbGiftingGroupYear.ParticipatingMembers();
    }

    private IEnumerable<Santa_User> GetOriginalSender(Santa_User dbSender, int? replyToMessageKey, bool checkReplySecurity)
    {
        return [GetOriginalMessage(dbSender, replyToMessageKey, checkReplySecurity)
            .Sender];
    }

    private IEnumerable<Santa_User> GetOriginalRecipients(Santa_User dbSender, int? replyToMessageKey, bool checkReplySecurity)
    {
        return GetOriginalMessage(dbSender, replyToMessageKey, checkReplySecurity)
            .Recipients.Select(x => x.RecipientSantaUser);
    }

    private Santa_Message GetOriginalMessage(Santa_User dbSender, int? replyToMessageKey, bool checkReplySecurity)
    {
        Santa_Message? originalMessage = DbContext.Santa_Messages
            .Where(x => x.MessageKey == replyToMessageKey)
            .FirstOrDefault();

        if (originalMessage != null && checkReplySecurity)
        {
            bool isRecipient = originalMessage.Recipients.Any(y => y.RecipientSantaUserKey == dbSender.SantaUserKey);

            if (!isRecipient)
            {
                isRecipient = originalMessage.RecipientType.AllowsFutureViewing()
                    && GetPossibleRecipients(originalMessage).Any(z => z.SantaUserKey == dbSender.SantaUserKey);
            }

            if (!isRecipient)
            {
                throw new AccessDeniedException();
            }
        }

        return originalMessage ?? throw new NotFoundException("Message");
    }

    private IEnumerable<Santa_User> GetGroupMembers(Santa_User dbSender)
    {
        return _dbGiftingGroupYear.GiftingGroup
            .OtherMembers(dbSender)
            .Select(x => x.SantaUser);
    }

    private IEnumerable<Santa_User> GetSpecificGroupMember(Santa_User dbSender, int? specificGroupMemberKey)
    {
        return GetGroupMembers(dbSender)
            .Where(x => x.SantaUserKey == specificGroupMemberKey);
    }
}
