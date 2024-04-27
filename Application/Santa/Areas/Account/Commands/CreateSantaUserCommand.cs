using Application.Santa.Global;
using Data.Entities.Santa;
using Data.Entities.Shared;
using FluentValidation.Results;
using Global.Abstractions.Global;
using Global.Abstractions.Santa.Areas.Account;
using Microsoft.AspNetCore.Identity;

namespace Application.Santa.Areas.Account.Commands;

public class CreateSantaUserCommand : BaseCommand<IRegisterSantaUser>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserStore<IdentityUser> _userStore;

    public CreateSantaUserCommand(IRegisterSantaUser item, UserManager<IdentityUser> userManager, IUserStore<IdentityUser> userStore) : base(item)
    {
        _userManager = userManager;
        _userStore = userStore;
    }

    public override async Task<ICommandResult<IRegisterSantaUser>> Handle()
    {
        Validator = new RegisterSantaValidator();
        Validation = Validator.Validate(Item);

        if (Validation.IsValid)
        {
            var globalUserDb = new Global_User
            {
                Forename = Item.Forename,
                MiddleNames = Item.MiddleNames,
                Surname = Item.Surname,
                Email = string.IsNullOrWhiteSpace(Item.Email) ? Item.Email : null,
                UserName = string.IsNullOrWhiteSpace(Item.UserName) ? Item.Email : Item.UserName,
            };

            var santaUserDb = new Santa_User
            {
                GlobalUserId = globalUserDb.Id,
                GlobalUser = globalUserDb
            };

            globalUserDb.SantaUser = santaUserDb;

            ModelContext.ChangeTracker.DetectChanges();

            IdentityResult result = await _userManager.CreateAsync(globalUserDb, Item.Password);

            if (result.Succeeded)
            {
                await _userStore.SetUserNameAsync(globalUserDb, Item.Email, CancellationToken.None);

                //if (!string.IsNullOrWhiteSpace(Item.Email))
                //{
                //    await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                //}

                Success = true;
                Item.Password = "";
                await ModelContext.SaveChangesAsync();
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    if (error.Description.ToLower().Contains("username"))
                    {
                        Validation.Errors.Add(new ValidationFailure(nameof(Item.UserName), error.Description));
                    }
                    else
                    {
                        Validation.Errors.Add(new ValidationFailure(nameof(Item.Password), error.Description));
                    }
                }
            }
        }

        return await Result();
    }
}
