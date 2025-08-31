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
    private IMailSettings _mailSettings;

    private string _notFound = "Not Found";

    public HealthchecksController(IConfiguration configuration, IServiceProvider services, SignInManager<IdentityUser> signInManager)
        : base(services, signInManager)
    {
        _configuration = configuration;
        _mailSettings = services.GetRequiredService<IMailSettings>();
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
        AddServer(model);

        string dabaseUserID = AddDatabaseUserID(model);
        string databasePassword = AddDatabasePassword(model);

        AddSymmetricKeyEnd(model);
        AddConnectionString(model, dabaseUserID, databasePassword);

        AddMailUserId(model);
        AddMailPassword(model);
        AddMailFrom(model);
        AddSmtpHost(model);
        AddSmtpPort(model);

        await AddQueryResult(model);
    }

    private void AddServer(HealthChecksVm model)
    {
        model.Server = GetConfigurationItem(ConfigurationSettings.DatabaseServer);
    }

    private string AddDatabaseUserID(HealthChecksVm model)
    {
        try
        {
            string userID = GetConfigurationItem(ConfigurationSettings.DatabaseUser);
            model.SafeDatabaseUserID = GetDisplayValue(userID);
            return userID; // return the original 'unsafe' value, so we can replace it later in the connection string
        }
        catch (Exception ex)
        {
            model.SafeDatabaseUserID = $"Could not obtain variable '{ConfigurationSettings.DatabaseUser}': {ex.Message}. Stack trace: {ex.StackTrace}";
            return _notFound;
        }
    }

    private string AddDatabasePassword(HealthChecksVm model)
    {
        try
        {
            string password = GetConfigurationItem(ConfigurationSettings.DatabasePassword);
            model.SafeDatabasePassword = GetDisplayValue(password);
            return password; // return the original 'unsafe' value, so we can replace it later in the connection string
        }
        catch (Exception ex)
        {
            model.SafeDatabasePassword = $"Could not obtain variable '{ConfigurationSettings.DatabasePassword}': {ex.Message}. Stack trace: {ex.StackTrace}";
            return _notFound;
        }
    }

    private void AddSymmetricKeyEnd(HealthChecksVm model)
    {
        model.SafeKeyEnd = GetSafeConfigurationItem(ConfigurationSettings.SymmetricKeyEnd);
    }

    private void AddConnectionString(HealthChecksVm model, string userID, string password)
    {
        try
        {
            model.DefaultConnection = _configuration.GetConnectionString("DefaultConnection");

            var connectionStringBuilder = new SqlConnectionStringBuilder(model.DefaultConnection);
            string connectionString = _configuration.GetConnectionString(connectionStringBuilder);

            if (userID.IsNotEmpty() && userID != _notFound && password.IsNotEmpty() && password != _notFound)
            {
                model.SafeConnectionString = connectionString.Replace(userID, model.SafeDatabaseUserID).Replace(password, model.SafeDatabasePassword);
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

    private void AddMailUserId(HealthChecksVm model)
    {
        model.SafeMailUserID = GetSafeConfigurationItem(ConfigurationSettings.EmailUserName);
    }

    private void AddMailPassword(HealthChecksVm model)
    {
        model.SafeMailPassword = GetSafeConfigurationItem(ConfigurationSettings.EmailPassword);
    }

    private void AddMailFrom(HealthChecksVm model)
    {
        model.SafeMailFrom = GetSafeConfigurationItem(ConfigurationSettings.EmailFromAddress);
    }

    private void AddSmtpHost(HealthChecksVm model)
    {
        model.SmtpHost = GetConfigurationItem(ConfigurationSettings.EmailHost);
    }

    private void AddSmtpPort(HealthChecksVm model)
    {
        model.SmtpPort = _mailSettings.Port;
    }

    private string GetSafeConfigurationItem(string configurationName)
    {
        return GetConfigurationItem(configurationName, true);
    }

    private string GetConfigurationItem(string configurationName, bool makeSafe = false)
    {
        try
        {
            string? value = _configuration[configurationName];
            return GetDisplayValue(value, makeSafe);
        }
        catch (Exception ex)
        {
            return $"Could not obtain variable '{configurationName}': {ex.Message}. Stack trace: {ex.StackTrace}";
        }
    }

    private string GetDisplayValue(string? value, bool makeSafe = true)
    {
        if (value.IsNotEmpty() && value != _notFound)
        {
            return makeSafe
                ? value.First() + "****" + value.Last()
                : value;
        }

        return _notFound;
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
