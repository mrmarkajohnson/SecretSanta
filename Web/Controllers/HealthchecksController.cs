using Application.Areas.Account.Queries;
using Application.Areas.GiftingGroup.Queries;
using Application.Areas.Messages.Commands;
using Application.Areas.Messages.ViewModels;
using Application.Shared.ViewModels;
using Data.Extensions;
using Global.Abstractions.Areas.Account;
using Global.Extensions.Exceptions;
using Global.Settings;
using Microsoft.Data.SqlClient;

namespace Web.Controllers;

public class HealthchecksController : BaseController
{
    private IConfiguration _configuration;

    public HealthchecksController(IConfiguration configuration, IServiceProvider services, SignInManager<IdentityUser> signInManager)
        : base(services, signInManager)
    {
        _configuration = configuration;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var model = new HealthChecksVm();
        await PopulateHealthchecksModel(model);
        return View(model);
    }

    private async Task PopulateHealthchecksModel(HealthChecksVm model)
    {
        string notFound = "Not Found";
        string? userID = notFound;
        string? password = notFound;

        AddServer(model);

        userID = AddUserID(model, notFound, userID);
        password = AddPassword(model, notFound, password);

        AddSymmetricKeyEnd(model);
        AddConnectionString(model, notFound, userID, password);

        await AddQueryResult(model);
    }

    private void AddServer(HealthChecksVm model)
    {
        try
        {
            model.Server = _configuration[ConfigurationSettings.DatabaseServer];
        }
        catch (Exception ex)
        {
            model.Server = $"Could not obtain variable '{ConfigurationSettings.DatabaseServer}': {ex.Message}. Stack trace: {ex.StackTrace}";
        }
    }

    private string? AddUserID(HealthChecksVm model, string notFound, string? userID)
    {
        try
        {
            userID = _configuration[ConfigurationSettings.DatabaseUser] ?? notFound;
            model.SafeUserID = userID == null ? "" : userID.First() + "****" + userID.Last();
        }
        catch (Exception ex)
        {
            model.SafeUserID = $"Could not obtain variable '{ConfigurationSettings.DatabaseUser}': {ex.Message}. Stack trace: {ex.StackTrace}";
        }

        return userID;
    }

    private string? AddPassword(HealthChecksVm model, string notFound, string? password)
    {
        try
        {
            password = _configuration[ConfigurationSettings.DatabasePassword] ?? notFound;
            model.SafePassword = password == null ? "" : password.First() + "****" + password.Last();
        }
        catch (Exception ex)
        {
            model.SafePassword = $"Could not obtain variable '{ConfigurationSettings.DatabasePassword}': {ex.Message}. Stack trace: {ex.StackTrace}";
        }

        return password;
    }

    private void AddSymmetricKeyEnd(HealthChecksVm model)
    {
        try
        {
            string? keyEnd = _configuration[ConfigurationSettings.SymmetricKeyEnd];
            model.SafeKeyEnd = keyEnd == null ? "" : keyEnd.First() + "****" + keyEnd.Last();
        }
        catch (Exception ex)
        {
            model.SafeKeyEnd = $"Could not obtain variable '{ConfigurationSettings.SymmetricKeyEnd}': {ex.Message}. Stack trace: {ex.StackTrace}";
        }
    }

    private void AddConnectionString(HealthChecksVm model, string notFound, string? userID, string? password)
    {
        try
        {
            model.DefaultConnection = _configuration.GetConnectionString("DefaultConnection");

            var connectionStringBuilder = new SqlConnectionStringBuilder(model.DefaultConnection);
            string connectionString = _configuration.GetConnectionString(connectionStringBuilder);

            if (userID.IsNotEmpty() && userID != notFound && password.IsNotEmpty() && password != notFound)
            {
                model.SafeConnectionString = connectionString.Replace(userID, model.SafeUserID).Replace(password, model.SafePassword);
            }
            else if (connectionString.IsNotEmpty())
            {
                model.SafeConnectionString = "Could not safely show connection string.";
            }
            else
            {
                model.SafeConnectionString = "Could not obtain connection string.";
            }
        }
        catch (Exception ex)
        {
            model.SafeConnectionString = $"Could not obtain connection string: {ex.Message}. Stack trace: {ex.StackTrace}";
        }
    }

    private async Task AddQueryResult(HealthChecksVm model)
    {
        try
        {
            int giftingGroupsCount = await Send(new GiftingGroupsCountQuery());

            if (giftingGroupsCount == 1)
            {
                model.QueryResult = $"There is 1 active gifting group.";
            }
            else
            {
                model.QueryResult = $"There are {giftingGroupsCount} active gifting groups.";
            }
        }
        catch (Exception ex)
        {
            model.QueryResult = $"Could not obtain count of gifting groups: {ex.Message}. Stack trace: {ex.StackTrace}";
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(HealthChecksVm model)
    {
        if (model == null)
        {
            model = new HealthChecksVm
            {
                PostResult = "Posted but model was empty."
            };

            await PopulateHealthchecksModel(model);
        }
        else if (string.IsNullOrWhiteSpace(model.Server))
        {
            model.PostResult = "Posted but contents were empty.";
        }
        else
        {
            model.SuccessMessage = "Posted successfully.";
            model.PostResult = "Posted form successfully.";
        }

        return View("Index", model);
    }

    [HttpGet]
    public async Task<IActionResult> SendTestEmail()
    {
        ISantaUser currentUser = await Send(new GetCurrentUserQuery(false));

        if (!currentUser.SystemAdmin)
            throw new AccessDeniedException("Only system administrators can send test e-mails.");

        var model = new SendTestEmailVm();
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SendTestEmail(SendTestEmailVm model)
    {
        ModelState.Clear();
        
        var commandResult = await Send(new SendTestEmailCommand(model), new SendTestEmailVmValidator());

        if (commandResult.Success)
        {
            model.SuccessMessage = "E-mail sent successfully.";
            model.RecipientEmailAddress = "";
        }

        return View(model);
    }
}
