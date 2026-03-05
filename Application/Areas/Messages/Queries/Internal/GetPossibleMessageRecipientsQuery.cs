using Global.Abstractions.Areas.Messages;
using static Global.Settings.MessageSettings;

namespace Application.Areas.Messages.Queries.Internal;

public sealed class GetPossibleMessageRecipientsQuery : GetMessagesBaseQuery<IList<Santa_User>>
{
    private readonly Santa_User _dbSender;
    private readonly Santa_GiftingGroupYear? _dbGiftingGroupYear;
    private readonly int? _replyToMessageKey;
    private readonly MessageRecipientType _recipientType;
    private readonly int? _specificGroupMemberKey;

    public GetPossibleMessageRecipientsQuery(Santa_User dbSender, Santa_GiftingGroupYear? dbGiftingGroupYear,
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
        IList<Santa_User> dbRecipients = GetPossibleRecipients(_dbGiftingGroupYear, _dbSender, _replyToMessageKey, 
            _recipientType, fromSanta: false, _specificGroupMemberKey, checkReplySecurity: true);

        return Result(dbRecipients);
    }
}
