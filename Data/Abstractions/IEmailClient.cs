using Data.Entities.Santa;
using Global.Abstractions.Areas.Messages;

namespace Data.Abstractions;

public interface IEmailClient
{
    FluentValidation.Results.ValidationResult SendMessage(Santa_Message dbMessage);
    FluentValidation.Results.ValidationResult SendMessage(ISantaMessage message, List<IEmailRecipient> recipients);
}