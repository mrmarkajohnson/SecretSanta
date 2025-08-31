using Application.Shared.BaseModels;
using Global.Abstractions.Areas.Messages;

namespace Application.Areas.Messages.BaseModels;

internal class EmailRecipient : UserNamesBase, IEmailRecipient
{
    public int MessageKey { get; set; }
    public int MessageRecipientKey { get; set; }    
}