using Application.Santa.Global;
using Global.Abstractions.Global;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Application.Santa.Areas.Account.Commands;

public class SetSecurityQuestionsCommand : BaseCommand<ISecurityQuestions>
{
    private readonly ClaimsPrincipal _user;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public SetSecurityQuestionsCommand(ISecurityQuestions item, 
        ClaimsPrincipal user, 
        UserManager<IdentityUser> userManager, 
        SignInManager<IdentityUser> signInManager) : base(item)
    {
        _user = user;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public override async Task<ICommandResult<ISecurityQuestions>> Handle()
    {
        Validator = new SecurityQuestionsValidator();
        Validation = Validator.Validate(Item);

        if (Validation.IsValid)
        {
            if (_signInManager.IsSignedIn(_user))
            {
                string? userId = _userManager.GetUserId(_user);
                if (userId != null)
                {
                    var globalUserDb = ModelContext.Global_Users
                        .FirstOrDefault(x => x.Id == userId);

                    if (globalUserDb != null)
                    {
                        globalUserDb.SecurityQuestion1 = Item.SecurityQuestion1;
                        globalUserDb.SecurityAnswer1 = Item.SecurityAnswer1;
                        globalUserDb.SecurityHint1 = Item.SecurityHint1;
                        globalUserDb.SecurityQuestion2 = Item.SecurityQuestion2;
                        globalUserDb.SecurityAnswer2 = Item.SecurityAnswer2;
                        globalUserDb.SecurityHint2 = Item.SecurityHint2;

                        await ModelContext.SaveChangesAsync();
                        Success = true;
                    }
                }
                else
                {

                }
            }
            else
            {

            }
        }

        return await Result();
    }
}
