using Application.Areas.GiftingGroup.Commands;
using static Global.Settings.GlobalSettings;

namespace Web.Areas.Account.Controllers;

public abstract class AccountBaseController : BaseController
{
    protected AccountBaseController(IServiceProvider services, SignInManager<IdentityUser> signInManager) 
        : base(services, signInManager)
    {
    }

    protected async Task<string?> HandleInvitation(bool keepIfNotSuccessful = false)
    {
        string? invitationId = TempData.Peek(InvitationId)?.ToString();

        if (invitationId.IsNotEmpty())
        {
            string participateUrl = GetParticipateUrl();
            var commandResult = await Send(new ReviewInvitationCommand(invitationId, participateUrl), null);

            if (commandResult.Success || !keepIfNotSuccessful)
            {
                TempData.Remove(InvitationId);
            }

            return commandResult.SuccessMessage;
        }

        return null;
    }
}
