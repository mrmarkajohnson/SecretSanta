using Global.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SecretSanta.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionStringBuilder = new SqlConnectionStringBuilder(builder.Configuration.GetConnectionString("DefaultConnection"));
connectionStringBuilder.UserID = builder.Configuration["DatabaseSettings:DevUserId"];
connectionStringBuilder.Password = builder.Configuration["DatabaseSettings:DevPassword"];
string connectionString = connectionStringBuilder.ConnectionString;

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedEmail = Identity.SignIn.RequireConfirmedEmail;
    options.SignIn.RequireConfirmedPhoneNumber = Identity.SignIn.RequireConfirmedPhoneNumber;
    options.SignIn.RequireConfirmedAccount = Identity.SignIn.RequireConfirmedAccount;
}).AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation(); //.AddMvcOptions(options => options.EnableEndpointRouting = false);

builder.Services.Configure<IdentityOptions>(Identity.ConfigureOptions);

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

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
    defaults: new { area = "" });

app.MapControllerRoute(
    name: "empty",
    pattern: "/",
    defaults: new { area = "", controller = "Home", action = "index" });

app.MapControllerRoute(
    name: "currentarea",
    pattern: "{controller=Home}/{action:exists}/{id?}",
    constraints: new { id = @"\d*" });

app.MapRazorPages();

FluentValidationConfiguration.SetFluentValidationOptions();

app.Run();