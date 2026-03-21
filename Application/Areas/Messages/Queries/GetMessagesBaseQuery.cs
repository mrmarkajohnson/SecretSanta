using AutoMapper.QueryableExtensions;
using Global.Abstractions.Areas.Messages;
using Global.Extensions.Exceptions;
using static Global.Settings.MessageSettings;

namespace Application.Areas.Messages.Queries;

public abstract class GetMessagesBaseQuery<TItem> : BaseQuery<TItem>
{
    /// <summary>
    /// Use getFirstMessageIfSent to return the first message in the chain that wasn't sent by the user, if it's a sent message
    /// Use checkReplySecurity to check that the user was the sender or a recipient, or could be (e.g. if using 'All Ever' types)
    /// </summary>
    protected Santa_Message GetOriginalMessage(int messageKey, Santa_User dbSantaUser, bool getFirstMessageIfSent, bool checkReplySecurity)
    {
        Santa_Message? dbOriginalMessage = DbContext.Santa_Messages
            .FirstOrDefault(x => x.MessageKey == messageKey);

        if (dbOriginalMessage != null)
        {
            bool canAccess = CanAccess(dbSantaUser, dbOriginalMessage, out bool isSender);

            if (isSender)
            {
                if (getFirstMessageIfSent)
                {
                    if (dbOriginalMessage.OriginalMessage != null && dbOriginalMessage.OriginalMessage.SenderKey != dbSantaUser.SantaUserKey)
                        return dbOriginalMessage;

                    if (dbOriginalMessage.ReplyToMessage != null && dbOriginalMessage.ReplyToMessage.SenderKey != dbSantaUser.SantaUserKey)
                        return dbOriginalMessage.ReplyToMessage;
                }

                return dbOriginalMessage;
            }
            else if (checkReplySecurity)
            {
                if (!canAccess)
                {
                    throw new AccessDeniedException();
                }
            }
        }

        return dbOriginalMessage ?? throw new NotFoundException("Message");
    }

    private bool CanAccess(Santa_User dbSantaUser, Santa_Message dbMessage, out bool isSender)
    {
        isSender = dbMessage.SenderKey == dbSantaUser.SantaUserKey;
        return isSender || IsRecipient(dbSantaUser, dbMessage);
    }

    private bool IsRecipient(Santa_User dbSantaUser, Santa_Message dbMessage)
    {
        bool isRecipient = dbMessage.Recipients.Any(y => y.RecipientSantaUserKey == dbSantaUser.SantaUserKey);

        if (!isRecipient)
        {
            isRecipient = IsIndirectRecipient(dbSantaUser, dbMessage);
        }

        return isRecipient;
    }

    protected IEnumerable<Santa_Message> IndirectMessages(Santa_User dbSantaUser)
    {
        return dbSantaUser.GiftingGroupYears
            .Where(YearGroupUserExpressions.IsActive(false))
            .SelectMany(x => x.GiftingGroupYear.Messages)
                .Where(MessageExpressions.IsActive())
                .ToList() // enables using a method
                .Where(y => IsIndirectRecipient(dbSantaUser, y));
    }

    protected bool IsIndirectRecipient(Santa_User dbSantaUser, Santa_Message dbOriginalMessage)
    {
        return dbOriginalMessage.RecipientType.AllowsFutureViewing(dbOriginalMessage.ShowAsFromSanta)
            && !dbOriginalMessage.Recipients.Any(x => x.RecipientSantaUserKey == dbSantaUser.SantaUserKey)
            && GetPossibleIndirectRecipients(dbOriginalMessage).Any(z => z.SantaUserKey == dbSantaUser.SantaUserKey);
    }

    #region Possible recipients

    protected IList<Santa_User> GetPossibleRecipients(Santa_GiftingGroupYear? dbGiftingGroupYear, Santa_User dbSender,
        int? replyToMessageKey, MessageRecipientType recipientType, bool fromSanta, int? firstRecipientKey, bool checkReplySecurity)
    {
        IEnumerable<Santa_User> dbPossibleRecipients = recipientType switch // may include the current user, who is removed below
        {
            MessageRecipientType.SystemAdmins
                => GetSystemAdmins(dbSender),
            MessageRecipientType.GroupAdmins
                => GetGroupAdmins(dbSender, dbGiftingGroupYear),
            MessageRecipientType.GiftRecipient
                => GetRecipient(dbSender, dbGiftingGroupYear, fromSanta),
            MessageRecipientType.Gifter
                => GetGifter(dbSender, dbGiftingGroupYear, fromSanta),
            MessageRecipientType.YearGroupCurrentMembers or MessageRecipientType.YearGroupAllEverMembers
                => GetParticipatingSantaUsers(dbGiftingGroupYear),
            MessageRecipientType.GroupCurrentMembers or MessageRecipientType.GroupAllEverMembers
                => GetGroupMembers(dbSender, dbGiftingGroupYear),
            MessageRecipientType.OriginalSender
                => GetOriginalSender(replyToMessageKey, dbSender, checkReplySecurity),
            MessageRecipientType.OriginalCurrentRecipients or MessageRecipientType.OriginalAllEverRecipients
                => GetOriginalRecipients(replyToMessageKey, dbSender, checkReplySecurity),
            MessageRecipientType.PotentialPartner
                => [], // should be handled elsewhere
            MessageRecipientType.SingleGroupMember
                => GetSpecificGroupMember(dbSender, firstRecipientKey, dbGiftingGroupYear),
            MessageRecipientType.SingleNonGroupMember
                => GetSpecificNonGroupMember(firstRecipientKey),
            _ => []
        };

        IList<Santa_User> dbRecipients = dbPossibleRecipients
            .Where(SantaUserExpressions.IsActive())
            .Where(x => x.SantaUserKey != dbSender.SantaUserKey)            
            .ToList();

        return dbRecipients;
    }

    private IList<Santa_User> GetPossibleIndirectRecipients(Santa_Message dbMessage)
    {
        return GetPossibleRecipients(dbMessage.GiftingGroupYear, dbMessage.Sender, dbMessage.ReplyToMessage?.MessageKey,
            dbMessage.RecipientType, dbMessage.ShowAsFromSanta, dbMessage.Recipients.FirstOrDefault()?.RecipientSantaUserKey, checkReplySecurity: false);
    }

    private IEnumerable<Santa_User> GetSystemAdmins(Santa_User dbSender)
    {
        return DbContext.Santa_Users
            .Where(SantaUserExpressions.IsActive())
            .Where(x => x.SantaUserKey != dbSender.SantaUserKey && x.GlobalUser.SystemAdmin);
    }

    private static IEnumerable<Santa_User> GetGroupAdmins(Santa_User dbSender, Santa_GiftingGroupYear? dbGiftingGroupYear)
    {
        return dbGiftingGroupYear?.GiftingGroup
            .OtherMembers(dbSender)
            .Where(x => x.GroupAdmin)
            .Select(x => x.SantaUser) ?? [];
    }

    private static IEnumerable<Santa_User> GetRecipient(Santa_User dbSender, Santa_GiftingGroupYear? dbGiftingGroupYear, bool fromSanta)
    {
        if (fromSanta)
            return [];

        return GetParticipants(dbGiftingGroupYear)
            .Where(x => x.SantaUserKey == dbSender.SantaUserKey)
            .Where(x => x.RecipientSantaUser != null)
            .Select(x => x.RecipientSantaUser!);
    }

    private static IEnumerable<Santa_User> GetGifter(Santa_User dbSender, Santa_GiftingGroupYear? dbGiftingGroupYear, bool fromSanta)
    {
        if (fromSanta)
            return [];
        
        return GetParticipants(dbGiftingGroupYear)
            .Where(x => x.RecipientSantaUserKey == dbSender.SantaUserKey)
            .Select(x => x.SantaUser);
    }

    private static IEnumerable<Santa_User> GetParticipatingSantaUsers(Santa_GiftingGroupYear? dbGiftingGroupYear)
    {
        return GetParticipants(dbGiftingGroupYear).Select(x => x.SantaUser);
    }

    private static List<Santa_YearGroupUser> GetParticipants(Santa_GiftingGroupYear? dbGiftingGroupYear)
    {
        return dbGiftingGroupYear?.ParticipatingMembers() ?? [];
    }

    private IEnumerable<Santa_User> GetOriginalSender(int? replyToMessageKey, Santa_User dbSantaUser, bool checkReplySecurity)
    {
        return [GetOriginalMessage(replyToMessageKey ?? 0, dbSantaUser, true, checkReplySecurity).Sender];
    }

    private IEnumerable<Santa_User> GetOriginalRecipients(int? replyToMessageKey, Santa_User dbSantaUser, bool checkReplySecurity)
    {
        return GetOriginalMessage(replyToMessageKey ?? 0, dbSantaUser, false, checkReplySecurity)
            .Recipients
            .Where(MessageRecipientExpressions.IsActive())
            .Select(x => x.RecipientSantaUser);
    }

    private static IEnumerable<Santa_User> GetGroupMembers(Santa_User dbSender, Santa_GiftingGroupYear? dbGiftingGroupYear)
    {
        return dbGiftingGroupYear?.GiftingGroup
            .OtherMembers(dbSender)
            .Select(x => x.SantaUser) ?? [];
    }

    private static IEnumerable<Santa_User> GetSpecificGroupMember(Santa_User dbSender, int? specificSantaUserKey, Santa_GiftingGroupYear? dbGiftingGroupYear)
    {
        return GetGroupMembers(dbSender, dbGiftingGroupYear)
            .Where(x => x.SantaUserKey == specificSantaUserKey);
    }

    private IEnumerable<Santa_User> GetSpecificNonGroupMember(int? specificSantaUserKey)
    {
        var dbMatchedSantaUser = DbContext.Santa_Users.FirstOrDefault(x => x.SantaUserKey == specificSantaUserKey);
        return dbMatchedSantaUser == null ? [] : [dbMatchedSantaUser];
    }

    #endregion Possible recipients

    protected static IEnumerable<Santa_Message> GetAllGroupMessages(Santa_User dbSantaUser)
    {
        return dbSantaUser.GiftingGroupYears
            .Where(YearGroupUserExpressions.IsActive(false))
            .SelectMany(x => x.GiftingGroupYear.Messages)
                .Where(MessageExpressions.IsActive());
    }

    protected IQueryable<T> GetSentMessages<T>(Santa_User dbCurrentSantaUser)
    {
        object parameters = new { CurrentSantaUserKey = dbCurrentSantaUser.SantaUserKey, UserKeysForVisibleEmail = dbCurrentSantaUser.UserKeysForVisibleEmail() };

        return dbCurrentSantaUser.SentMessages
            .Where(MessageExpressions.IsActive())
            .Where(x => x.RecipientType is not MessageRecipientType.PotentialPartner or MessageRecipientType.SingleNonGroupMember)
            .OrderByDescending(x => x.DateCreated)
            .AsQueryable()
            .ProjectTo<T>(Mapper.ConfigurationProvider, parameters);
    }

    protected void AddPreviousMessages(IReadMessage message, Santa_User dbCurrentSantaUser, IEnumerable<Santa_Message>? allGroupMessages = null)
    {
        List<Santa_Message> previousMessages = GetPreviousMessages(message.MessageKey, dbCurrentSantaUser, allGroupMessages);
        object parameters = new { CurrentSantaUserKey = dbCurrentSantaUser.SantaUserKey, UserKeysForVisibleEmail = dbCurrentSantaUser.UserKeysForVisibleEmail() };

        var previousReceivedMessages = previousMessages
            .Where(x => x.SenderKey != dbCurrentSantaUser.SantaUserKey)
            .AsQueryable()
            .ProjectTo<ISantaMessage>(Mapper.ConfigurationProvider, new { parameters });

        var previousSentMessages = previousMessages
            .Where(x => x.SenderKey == dbCurrentSantaUser.SantaUserKey)
            .AsQueryable()
            .ProjectTo<IReadSentMessage>(Mapper.ConfigurationProvider, new { parameters });

        message.PreviousMessages = previousReceivedMessages.Union(previousSentMessages)
            .OrderByDescending(x => x.Sent)
            .ToList();
    }

    private static List<Santa_Message> GetPreviousMessages(int messageKey, Santa_User dbSantaUser, IEnumerable<Santa_Message>? allGroupMessages = null)
    {
        allGroupMessages ??= GetAllGroupMessages(dbSantaUser);

        List<Santa_Message> previousMessages = allGroupMessages
            .Where(x => x.Replies
                .Where(MessageExpressions.IsActive())
                .Any(y => y.MessageKey == messageKey))
            .ToList();

        var olderMessages = previousMessages;

        while (olderMessages.Count > 0)
        {
            olderMessages = allGroupMessages
                .Where(x => olderMessages.Any(y => y.ReplyToMessage?.MessageKey == x.MessageKey))
                .ToList();

            if (olderMessages.Count > 0)
            {
                previousMessages.AddRange(olderMessages);
            }
        }

        return previousMessages.DistinctBy(x => x.MessageKey).ToList();
    }

    protected void AddLaterMessages(IReadMessage message, Santa_User dbCurrentSantaUser, IEnumerable<Santa_Message>? allGroupMessages = null)
    {
        List<Santa_Message> laterMessages = GetLaterMessages(message.MessageKey, dbCurrentSantaUser, allGroupMessages);
        object parameters = new { CurrentSantaUserKey = dbCurrentSantaUser.SantaUserKey, UserKeysForVisibleEmail = dbCurrentSantaUser.UserKeysForVisibleEmail() };

        var laterReceivedMessages = laterMessages
            .Where(x => x.SenderKey != dbCurrentSantaUser.SantaUserKey)
            .AsQueryable()
            .ProjectTo<ISantaMessage>(Mapper.ConfigurationProvider, new { parameters });

        var laterSentMessages = laterMessages
            .Where(x => x.SenderKey == dbCurrentSantaUser.SantaUserKey)
            .AsQueryable()
            .ProjectTo<IReadSentMessage>(Mapper.ConfigurationProvider, new { parameters });

        message.LaterMessages = laterReceivedMessages.Union(laterSentMessages)
            .OrderByDescending(x => x.Sent)
            .ToList();
    }

    private List<Santa_Message> GetLaterMessages(int messageKey, Santa_User dbSantaUser, IEnumerable<Santa_Message>? allGroupMessages = null)
    {
        allGroupMessages ??= GetAllGroupMessages(dbSantaUser);
        var accessibleGroupMessages = allGroupMessages.ToList().Where(x => CanAccess(dbSantaUser, x, out bool _)).ToList();

        List<Santa_Message> laterMessages = accessibleGroupMessages
            .Where(x => x.ReplyToMessageKey == messageKey)
            .ToList();

        var newerMessages = laterMessages;

        while (newerMessages.Count > 0)
        {
            newerMessages = accessibleGroupMessages
                .Where(x => newerMessages.Any(y => x.ReplyToMessageKey == y.MessageKey))                
                .ToList();

            if (newerMessages.Count > 0)
            {
                laterMessages.AddRange(newerMessages);
            }
        }

        return laterMessages.DistinctBy(x => x.MessageKey).ToList();
    }
}
