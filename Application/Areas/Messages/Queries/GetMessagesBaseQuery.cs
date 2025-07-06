using Application.Shared.Requests;
using AutoMapper.QueryableExtensions;
using Global.Abstractions.Areas.Messages;
using Global.Extensions.Exceptions;
using static Global.Settings.MessageSettings;

namespace Application.Areas.Messages.Queries;

public abstract class GetMessagesBaseQuery<TItem> : BaseQuery<TItem>
{
    /// <summary>
    /// Use dbCheckRecipient to check that the user was a recipient, or could be (e.g. if using 'All Ever' types)
    /// </summary>
    protected Santa_Message GetOriginalMessage(int messageKey, Santa_User? dbCheckRecipient)
    {
        Santa_Message? dbOriginalMessage = DbContext.Santa_Messages
            .Where(x => x.MessageKey == messageKey)
            .FirstOrDefault();

        if (dbOriginalMessage != null && dbCheckRecipient != null)
        {
            bool isRecipient = dbOriginalMessage.Recipients.Any(y => y.RecipientSantaUserKey == dbCheckRecipient.SantaUserKey);

            if (!isRecipient)
            {
                isRecipient = IsIndirectRecipient(dbCheckRecipient, dbOriginalMessage);
            }

            if (!isRecipient)
            {
                throw new AccessDeniedException();
            }
        }

        return dbOriginalMessage ?? throw new NotFoundException("Message");
    }

    protected IEnumerable<Santa_Message> IndirectMessages(Santa_User dbSantaUser)
    {
        return dbSantaUser.GiftingGroupYears
            .SelectMany(x => x.GiftingGroupYear.Messages)
            .ToList()
            .Where(y => IsIndirectRecipient(dbSantaUser, y));
    }

    protected bool IsIndirectRecipient(Santa_User dbSantaUser, Santa_Message dbOriginalMessage)
    {
        return dbOriginalMessage.RecipientType.AllowsFutureViewing()
            && !dbOriginalMessage.Recipients.Any(x => x.RecipientSantaUserKey == dbSantaUser.SantaUserKey)
            && GetPossibleRecipients(dbOriginalMessage).Any(z => z.SantaUserKey == dbSantaUser.SantaUserKey);
    }

    #region Possible recipients

    protected IList<Santa_User> GetPossibleRecipients(Santa_GiftingGroupYear? dbGiftingGroupYear, Santa_User dbSender,
        int? replyToMessageKey, MessageRecipientType recipientType, int? specificGroupMemberKey, bool checkReplySecurity)
    {
        IEnumerable<Santa_User> dbPossibleRecipients = recipientType switch // may include the current user, who is removed below
        {
            MessageRecipientType.SystemAdmins => [], // TODO: Add System Admins once they exist
            MessageRecipientType.GroupAdmins
                => GetGroupAdmins(dbSender, dbGiftingGroupYear),
            MessageRecipientType.GiftRecipient
                => GetRecipient(dbSender, dbGiftingGroupYear),
            MessageRecipientType.Gifter
                => GetGifter(dbSender, dbGiftingGroupYear),
            MessageRecipientType.YearGroupCurrentMembers or MessageRecipientType.YearGroupAllEverMembers
                => GetParticipatingSantaUsers(dbGiftingGroupYear),
            MessageRecipientType.GroupCurrentMembers or MessageRecipientType.GroupAllEverMembers
                => GetGroupMembers(dbSender, dbGiftingGroupYear),
            MessageRecipientType.OriginalSender
                => GetOriginalSender(replyToMessageKey, checkReplySecurity ? dbSender : null),
            MessageRecipientType.OriginalCurrentRecipients or MessageRecipientType.OriginalAllEverRecipients
                => GetOriginalRecipients(replyToMessageKey, checkReplySecurity ? dbSender : null),
            MessageRecipientType.PotentialPartner
                => [], // should be handled elsewhere
            MessageRecipientType.SingleGroupMember
                => GetSpecificGroupMember(dbSender, specificGroupMemberKey, dbGiftingGroupYear),
            _ => []
        };

        IList<Santa_User> dbRecipients = dbPossibleRecipients
            .Where(x => x.SantaUserKey != dbSender.SantaUserKey)
            .Where(x => x.DateArchived == null && x.DateDeleted == null)
            .ToList();

        return dbRecipients;
    }

    private IList<Santa_User> GetPossibleRecipients(Santa_Message dbMessage)
    {
        return GetPossibleRecipients(dbMessage.GiftingGroupYear, dbMessage.Sender, dbMessage.ReplyTo?.OriginalMessageKey,
            dbMessage.RecipientType, dbMessage.Recipients.FirstOrDefault()?.RecipientSantaUserKey, false);
    }

    private IEnumerable<Santa_User> GetGroupAdmins(Santa_User dbSender, Santa_GiftingGroupYear? dbGiftingGroupYear)
    {
        return dbGiftingGroupYear?.GiftingGroup
            .OtherMembers(dbSender)
            .Where(x => x.GroupAdmin)
            .Select(x => x.SantaUser) ?? [];
    }

    private IEnumerable<Santa_User> GetRecipient(Santa_User dbSender, Santa_GiftingGroupYear? dbGiftingGroupYear)
    {
        return GetParticipants(dbGiftingGroupYear)
            .Where(x => x.SantaUserKey == dbSender.SantaUserKey)
            .Where(x => x.RecipientSantaUser != null)
            .Select(x => x.RecipientSantaUser);
    }

    private IEnumerable<Santa_User> GetGifter(Santa_User dbSender, Santa_GiftingGroupYear? dbGiftingGroupYear)
    {
        return GetParticipants(dbGiftingGroupYear)
            .Where(x => x.RecipientSantaUserKey == dbSender.SantaUserKey)
            .Select(x => x.SantaUser);
    }

    private IEnumerable<Santa_User> GetParticipatingSantaUsers(Santa_GiftingGroupYear? dbGiftingGroupYear)
    {
        return GetParticipants(dbGiftingGroupYear).Select(x => x.SantaUser);
    }

    private List<Santa_YearGroupUser> GetParticipants(Santa_GiftingGroupYear? dbGiftingGroupYear)
    {
        return dbGiftingGroupYear?.ParticipatingMembers() ?? [];
    }

    private IEnumerable<Santa_User> GetOriginalSender(int? replyToMessageKey, Santa_User? dbCheckRecipient)
    {
        return [GetOriginalMessage(replyToMessageKey ?? 0, dbCheckRecipient).Sender];
    }

    private IEnumerable<Santa_User> GetOriginalRecipients(int? replyToMessageKey, Santa_User? dbCheckRecipient)
    {
        return GetOriginalMessage(replyToMessageKey ?? 0, dbCheckRecipient)
            .Recipients.Select(x => x.RecipientSantaUser);
    }

    private IEnumerable<Santa_User> GetGroupMembers(Santa_User dbSender, Santa_GiftingGroupYear? dbGiftingGroupYear)
    {
        return dbGiftingGroupYear?.GiftingGroup
            .OtherMembers(dbSender)
            .Select(x => x.SantaUser) ?? [];
    }

    private IEnumerable<Santa_User> GetSpecificGroupMember(Santa_User dbSender, int? specificGroupMemberKey, Santa_GiftingGroupYear? dbGiftingGroupYear)
    {
        return GetGroupMembers(dbSender, dbGiftingGroupYear)
            .Where(x => x.SantaUserKey == specificGroupMemberKey);
    }

    #endregion Possible recipients

    protected static IEnumerable<Santa_Message> GetAllGroupMessages(Santa_User dbSantaUser)
    {
        return dbSantaUser.GiftingGroupYears
            .SelectMany(x => x.GiftingGroupYear.Messages);
    }

    protected IQueryable<T> GetSentMessages<T>(Santa_User dbSantaUser)
    {
        return dbSantaUser.SentMessages
            .Where(x => x.RecipientType != MessageRecipientType.PotentialPartner)
            .OrderByDescending(x => x.DateCreated)
            .AsQueryable()
            .ProjectTo<T>(Mapper.ConfigurationProvider);
    }

    protected void AddPreviousMessages(IReadMessage message, Santa_User dbSantaUser, IEnumerable<Santa_Message>? allGroupMessages = null)
    {
        List<Santa_Message> previousMessages = GetPreviousMessages(message.MessageKey, dbSantaUser, allGroupMessages);

        message.PreviousMessages = previousMessages
            .AsQueryable()
            .ProjectTo<ISantaMessage>(Mapper.ConfigurationProvider, new { CurrentSantaUserKey = dbSantaUser.SantaUserKey })
            .OrderByDescending(x => x.Sent)
            .ToList();
    }

    private static List<Santa_Message> GetPreviousMessages(int messageKey, Santa_User dbSantaUser, IEnumerable<Santa_Message>? allGroupMessages = null)
    {
        allGroupMessages ??= GetAllGroupMessages(dbSantaUser);

        List<Santa_Message> previousMessages = allGroupMessages
            .Where(x => x.Replies.Any(x => x.ReplyMessageKey == messageKey))
            .ToList();

        var olderMessages = previousMessages;

        while (olderMessages.Count > 0)
        {
            olderMessages = allGroupMessages
                .Where(x => olderMessages.Any(y => y.ReplyTo?.OriginalMessageKey == x.MessageKey))
                .ToList();

            if (olderMessages.Count > 0)
            {
                previousMessages.AddRange(olderMessages);
            }
        }

        return previousMessages;
    }
}
