using System.ComponentModel.DataAnnotations;

namespace Global.Abstractions.Areas.Messages;

public interface ISantaMessageBase : IMessageBase
{
    int MessageKey { get; }
    public DateTime Sent { get; }

    [Display(Name = "For Group")]
    string? GroupName { get; }

    bool ShowAsFromSanta { get; }
}
