using Application.Areas.GiftingGroup.ViewModels;
using Application.Shared.Requests;
using Global.Abstractions.Areas.GiftingGroup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Areas.GiftingGroup.Commands;

public class SendInvitationCommand<TItem> : BaseCommand<TItem> where TItem : ISendGroupInvitation
{
    public SendInvitationCommand(TItem item) : base(item)
    {
    }

    protected override Task<ICommandResult<TItem>> HandlePostValidation()
    {
        Santa_User dbCurrentSantaUser = GetCurrentSantaUser(s => s.GiftingGroupLinks);

        


    }
}
