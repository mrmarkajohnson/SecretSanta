using System.ComponentModel.DataAnnotations;
using static Global.Settings.MessageSettings;

namespace Global.Abstractions.Areas.Messages;

public interface IHasMessageRecipientType
{
    [Display(Name = "To")]
    MessageRecipientType RecipientType { get; set; }
}
