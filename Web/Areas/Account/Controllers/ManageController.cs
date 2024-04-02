using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ViewLayer.Models.Account;

namespace Web.Areas.Account.Controllers;

[Area("Account")]
public class ManageController : Controller
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserStore<IdentityUser> _userStore;

    public ManageController(UserManager<IdentityUser> userManager,
        IUserStore<IdentityUser> userStore,
        SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _userStore = userStore;
        _signInManager = signInManager;
    }

    [HttpGet]
    public IActionResult Register(string? returnUrl = null)
    {
        var model = new RegisterVm
        {
            ReturnUrl = returnUrl,
            Forename = "",
            Surname = "",
            Password = "",
            ConfirmPassword = ""
            //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterVm model)
    {
        model.ReturnUrl ??= Url.Content("~/");
        //model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        if (ModelState.IsValid)
        {
            //var user = CreateUser();

            //await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
            //await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

            //IdentityResult result = await _userManager.CreateAsync(user);

            //if (result.Succeeded)
            //{
            //    if (_userManager.Options.SignIn.RequireConfirmedAccount)
            //    {
            //        return RedirectToPage("RegisterConfirmation", new { email = model.Email, returnUrl = model.ReturnUrl });
            //    }
            //    else
            //    {
            //        await _signInManager.SignInAsync(user, isPersistent: false);
            //        return LocalRedirect(model.ReturnUrl);
            //    }
            //}

            //foreach (var error in result.Errors)
            //{
            //    ModelState.AddModelError(string.Empty, error.Description);
            //}
        }

        // If we got this far, something failed, redisplay form
        return View();
    }

    //private IdentityUser CreateUser()
    //{
    //    try
    //    {
    //        return Activator.CreateInstance<IdentityUser>();
    //    }
    //    catch
    //    {
    //        throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
    //            $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
    //            $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
    //    }
    //}

    //private IUserEmailStore<IdentityUser> GetEmailStore()
    //{
    //    if (!_userManager.SupportsUserEmail)
    //    {
    //        throw new NotSupportedException("The default UI requires a user store with email support.");
    //    }
    //    return (IUserEmailStore<IdentityUser>)_userStore;
    //}
}
