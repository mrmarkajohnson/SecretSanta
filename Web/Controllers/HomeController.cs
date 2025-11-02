using Application.Areas.GiftingGroup.Queries;
using Application.Shared.ViewModels;
using Global.Abstractions.Areas.GiftingGroup;
using Global.Extensions.Exceptions;
using Global.Settings;
using System.Diagnostics;

namespace Web.Controllers;

public sealed class HomeController : BaseController
{
    public HomeController(IServiceProvider services, SignInManager<IdentityUser> signInManager) : base(services, signInManager)
    {
    }

    public async Task<IActionResult> Index(string? successMessage = null, string? invitationId = null)
    {
        string? invitationWaitMessage = null;

        if (invitationId.IsNotEmpty())
        {
            try
            {
                IReviewGroupInvitation invitation = await Send(new GetInvitationQuery(invitationId));
                TempData[TempDataNames.InvitationGuid] = invitation.InvitationGuid;
                TempData[TempDataNames.InvitationWaitMessage] = invitationWaitMessage = GetInvitationWaitMessage(invitation);
            }
            catch (NotFoundException nfx)
            {
                HandleInvitationError(nfx);
                return RedirectHome();
            }
            catch (AccessDeniedException adx)
            {
                HandleInvitationError(adx);
                return RedirectHome();
            }
            catch
            {
            }
        }
        else
        {
            invitationWaitMessage = TempData.Peek(TempDataNames.InvitationWaitMessage)?.ToString();
        }

        if (invitationWaitMessage.IsNotEmpty())
        {
            invitationWaitMessage += " You can review it after logging in or registering.";
            HomeModel.InvitationWaitMessage = invitationWaitMessage;
        }

        HomeModel.SuccessMessage = successMessage;         
        HomeModel.InvitationError = TempData[TempDataNames.InvitationError]?.ToString();
        TempData.Remove(TempDataNames.InvitationError); // just in case
        
        return View(HomeModel);
    }

    public IActionResult About()
    {
        return View();
    }

    public IActionResult FAQs()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}