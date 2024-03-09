using Data.Entities.Data.Santa;
using Data.Helpers;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SecretSanta.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {            
    }

    public DbSet<Santa_GiftingGroup> Santa_GiftingGroups => Set<Santa_GiftingGroup>();
    public DbSet<Santa_GiftingGroupUser> Santa_GiftingGroupUsers => Set<Santa_GiftingGroupUser>();
    public DbSet<Santa_GiftingGroupYear> Santa_GiftingGroupYears => Set<Santa_GiftingGroupYear>();
    public DbSet<Santa_Message> Santa_Messages => Set<Santa_Message>();
    public DbSet<Santa_MessageRecipient> Santa_MessageRecipients => Set<Santa_MessageRecipient>();
    public DbSet<Santa_MessageReply> Santa_MessageReplies => Set<Santa_MessageReply>();
    public DbSet<Santa_PartnerLink> Santa_Partners => Set<Santa_PartnerLink>();
    public DbSet<Santa_Suggestion> Santa_Suggestions => Set<Santa_Suggestion>();
    public DbSet<Santa_User> Santa_Users => Set<Santa_User>();
    public DbSet<Santa_YearGroupUser> Santa_YearGroupUsers => Set<Santa_YearGroupUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ModelHelper.OnModelCreating(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    if (!optionsBuilder.IsConfigured)
    //    {
    //        IConfigurationRoot configuration = new ConfigurationBuilder()
    //        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    //        .AddJsonFile("appsettings.json")
    //        .Build();

    //        var connectionStringBuilder = new SqlConnectionStringBuilder(configuration.GetConnectionString("DefaultConnection"));
    //        connectionStringBuilder.UserID = configuration["DatabaseSettings:DevUserId"];
    //        connectionStringBuilder.Password = configuration["DatabaseSettings:DevPassword"];
    //        string connectionString = connectionStringBuilder.ConnectionString;

    //        optionsBuilder.UseSqlServer(configuration.GetConnectionString(connectionString));
    //    }
    //}
}        