using AutoMapper;
using Global.Extensions.Services;
using Global.Validation;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SecretSanta.Data;
using System.Reflection;
using Web.GlobalErrorHandling;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionStringBuilder = new SqlConnectionStringBuilder(builder.Configuration.GetConnectionString("DefaultConnection"));
connectionStringBuilder.UserID = builder.Configuration["DatabaseSettings:DevUserId"];
connectionStringBuilder.Password = builder.Configuration["DatabaseSettings:DevPassword"];
string connectionString = connectionStringBuilder.ConnectionString;

builder.Services.AddDbContext<ApplicationDbContext>(options => options
    .UseLazyLoadingProxies()
    .UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<ApplicationDbContext>();

if (builder.Services.Any(f => f.ServiceType == typeof(IValidationAttributeAdapterProvider)))
{
    builder.Services.Remove(builder.Services.Single(f => f.ServiceType == typeof(IValidationAttributeAdapterProvider)));
}

builder.Services.AddSingleton<IValidationAttributeAdapterProvider, CustomValidationAttributeAdapterProvider>();

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddTransient<BaseActionFilter>();
builder.Services.AddRazorPages().AddMvcOptions(options => options.Filters.AddService<BaseActionFilter>());

string[] mapperAssemblyNames = ["Application", "ViewModels"];
Assembly[] mapperAssemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => mapperAssemblyNames.Contains(x.GetName().Name)).ToArray();
var profiles = mapperAssemblies.SelectMany(x => x.GetTypes().Where(x => typeof(Profile).IsAssignableFrom(x))).ToArray();
builder.Services.ConfigureAutoMapperProfiles();

builder.Services.Configure<IdentityOptions>(IdentityVal.ConfigureOptions);

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);

    options.LoginPath = "/Account/Home/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}",
    constraints: new { id = @"\d*" });

app.MapControllerRoute(
    name: "root",
    pattern: "/{controller:exists}/{action:exists}",
    defaults: new { area = string.Empty });

app.MapControllerRoute(
    name: "slash",
    pattern: "/",
    defaults: new { area = string.Empty, controller = "Home", action = "index" });

app.MapControllerRoute(
    name: "empty",
    pattern: "",
    defaults: new { area = string.Empty, controller = "Home", action = "index" });

app.MapControllerRoute(
    name: "currentarea",
    pattern: "{controller=Home}/{action:exists}/{id?}",
    constraints: new { id = @"\d*" });

app.MapControllerRoute(
    name: "home",
    pattern: "home/{*url}",
    defaults: new { controller = "Home", action = "Index" });

app.MapControllerRoute(
    name: "index",
    pattern: "index",
    defaults: new { action = "Index" });

app.MapRazorPages();

FluentValidationConfiguration.SetFluentValidationOptions();

app.UseMiddleware(typeof(GlobalRequestProcessor));

app.Run();