using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ViewLayer.Models.Account;

namespace Web.Areas.Account.Controllers;

[Area("Account")]
public class HomeController : Controller
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserStore<IdentityUser> _userStore;

    public HomeController(UserManager<IdentityUser> userManager,
        IUserStore<IdentityUser> userStore,
        SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _userStore = userStore;
        _signInManager = signInManager;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Login(string? returnUrl = null)
    {
        //if (!string.IsNullOrEmpty(ErrorMessage))
        //{
        //    ModelState.AddModelError(string.Empty, ErrorMessage);
        //}

        var model = new LoginVm
        {
            EmailOrUserName = "",
            Password = "",
            //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList(),
            ReturnUrl = returnUrl ?? Url.Content("~/")
        };

        // Clear the existing external cookie to ensure a clean login process
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);        

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginVm model)
    {
        model.ReturnUrl ??= Url.Content("~/");

        //model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        if (ModelState.IsValid)
        {
            //This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            var result = await _signInManager.PasswordSignInAsync(model.EmailOrUserName, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                //_logger.LogInformation("User logged in.");
                return LocalRedirect(model.ReturnUrl);
            }
            if (result.RequiresTwoFactor)
            {
                return RedirectToPage("./LoginWith2fa", new { model.ReturnUrl,model.RememberMe });
            }
            if (result.IsLockedOut)
            {
                //_logger.LogWarning("User account locked out.");
                return RedirectToPage("./Lockout");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                //return Page();
            }
        }

        model.Password = "";
        return View(model);
    }
}