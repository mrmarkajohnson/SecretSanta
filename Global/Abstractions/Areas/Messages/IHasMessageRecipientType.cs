using static Global.Settings.MessageSettings;

namespace Global.Abstractions.Areas.Messages;

public interface IHasMessageRecipientType
{
    MessageRecipientType RecipientType { get; }
}
