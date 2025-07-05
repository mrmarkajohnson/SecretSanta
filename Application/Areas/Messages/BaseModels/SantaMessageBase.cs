using Global.Abstractions.Areas.Messages;
using System.ComponentModel.DataAnnotations;

namespace Application.Areas.Messages.BaseModels;

public class SantaMessageBase : MessageBase, ISantaMessageBase
{
    public int MessageKey { get; set; }
    public DateTime Sent { get; set; }
    public bool ShowAsFromSanta { get; set; }

    [Display(Name = "For Group")]
    public string? GroupName { get; set; }
}
