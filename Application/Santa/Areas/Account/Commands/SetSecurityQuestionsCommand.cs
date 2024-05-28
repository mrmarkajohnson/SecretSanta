using FluentValidation.Results;
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
                    var globalUserDb = ModelContext.Global_Users.FirstOrDefault(x => x.Id == userId);

                    if (globalUserDb != null)
                    {
                        globalUserDb.SecurityQuestion1 = Item.SecurityQuestion1;
                        globalUserDb.SecurityAnswer1 = EncryptionHelper.OneWayEncrypt(Item.SecurityAnswer1?.ToLower() ?? "", globalUserDb);
                        globalUserDb.SecurityHint1 = EncryptionHelper.TwoWayEncrypt(Item.SecurityHint1, false);
                        globalUserDb.SecurityQuestion2 = Item.SecurityQuestion2;
                        globalUserDb.SecurityAnswer2 = EncryptionHelper.OneWayEncrypt(Item.SecurityAnswer2?.ToLower() ?? "", globalUserDb);
                        globalUserDb.SecurityHint2 = EncryptionHelper.TwoWayEncrypt(Item.SecurityHint2, false);

                        await ModelContext.SaveChangesAsync();
                        Success = true;
                    }
                }
                else
                {
                    Validation.Errors.Add(new ValidationFailure(string.Empty, "User not found. Please log in again."));
                }
            }
            else
            {
                Validation.Errors.Add(new ValidationFailure(string.Empty, "User not found. Please log in again."));
            }
        }

        return await Result();
    }
}
