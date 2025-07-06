using System.ComponentModel.DataAnnotations;

namespace Global.Abstractions.Areas.Messages;

public interface ISantaMessageBase : IMessageBase
{
    int MessageKey { get; set; }
    public DateTime Sent { get; set; }

    [Display(Name = "For Group")]
    string? GroupName { get; }

    bool ShowAsFromSanta { get; }
}
