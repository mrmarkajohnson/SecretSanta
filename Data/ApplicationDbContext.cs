using Data.Attributes;
using Data.DummyImplementations;
using Data.Entities.Santa;
using Data.Entities.Shared;
using Data.Helpers;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using static Global.Settings.GlobalSettings;

namespace SecretSanta.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext() : this(new DbContextOptions<ApplicationDbContext>())
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public string? CurrentUserId { get; set; }

    public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ChangeTracker.DetectChanges();
        AddAuditTrails();
        return await base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        ChangeTracker.DetectChanges();
        AddAuditTrails();
        return base.SaveChanges();
    }

    private void AddAuditTrails()
    {
        var modifiedAuditableEntities = ChangeTracker.Entries()
            .Where(e => e.Entity is IAuditableEntity)
            .Where(e => e.State == EntityState.Added
                || e.State == EntityState.Modified
                || e.State == EntityState.Deleted)
            .ToList();

        foreach (var modifiedEntity in modifiedAuditableEntities)
        {
            if (modifiedEntity.Entity is IAuditableEntity auditableEntity)
            {
                AddAuditTrail(modifiedEntity, auditableEntity, modifiedEntity.State);
            }
        }

        ChangeTracker.DetectChanges();
    }

    private void AddAuditTrail<TAuditableEntity>(EntityEntry modifiedEntry, TAuditableEntity auditableEntity, EntityState state)
        where TAuditableEntity : IAuditableEntity
    {
        var changes = GetChanges(modifiedEntry);

        AuditAction action = GetAuditAction(auditableEntity, state, changes);

        auditableEntity.AddAuditEntry(new Dummy_AuditEntry
        {
            Action = action,
            GlobalUserId = CurrentUserId ?? null
        }, changes);
    }

    private static AuditAction GetAuditAction<TAuditableEntity>(TAuditableEntity auditableEntity, EntityState state, IList<IAuditBaseChange> changes)
        where TAuditableEntity : IAuditableEntity
    {
        AuditAction action = state switch
        {
            EntityState.Added => AuditAction.Create,
            EntityState.Modified => AuditAction.Update, // initially
            EntityState.Deleted => AuditAction.Delete,
            _ => AuditAction.Update
        };

        if (action == AuditAction.Update)
        {
            if (auditableEntity is IDeletableEntity deletableEntity && deletableEntity.DateDeleted != null
                && changes.Any(x => x.ColumnName == nameof(deletableEntity.DateDeleted)))
            {
                action = AuditAction.Delete;
            }
            else if (auditableEntity is IArchivableEntity archivableEntity && archivableEntity.DateArchived != null
                && changes.Any(x => x.ColumnName == nameof(archivableEntity.DateArchived)))
            {
                action = AuditAction.Archive;
            }
        }

        return action;
    }

    private IList<IAuditBaseChange> GetChanges(EntityEntry entity)
    {
        var changes = new List<IAuditBaseChange>();

        foreach (IProperty property in entity.OriginalValues.Properties)
        {
            var oldValue = entity.OriginalValues[property];
            var newValue = entity.CurrentValues[property];

            if (!Equals(oldValue, newValue))
            {
                var auditAttribute = property.PropertyInfo?.GetCustomAttribute<AuditAttribute>(true);

                if (auditAttribute != null && auditAttribute.NotAudited)
                {
                    continue;
                }

                changes.Add(new Dummy_AuditChange
                {
                    ColumnName = property.Name,
                    DisplayName = !string.IsNullOrWhiteSpace(auditAttribute?.Name) ? auditAttribute.Name : property.Name.SplitPascalCase(),
                    OldValue = oldValue?.ToString() ?? string.Empty,
                    NewValue = newValue?.ToString() ?? string.Empty
                });
            }
        }
        return changes;
    }

    public DbSet<Global_User> Global_Users => Set<Global_User>();
    public DbSet<Global_User_Audit> Global_User_Audit => Set<Global_User_Audit>();
    public DbSet<Global_User_AuditChange> Global_User_AuditChanges => Set<Global_User_AuditChange>();
    public DbSet<Santa_GiftingGroup> Santa_GiftingGroups => Set<Santa_GiftingGroup>();
    public DbSet<Santa_GiftingGroup_Audit> Santa_GiftingGroup_Audit => Set<Santa_GiftingGroup_Audit>();
    public DbSet<Santa_GiftingGroup_AuditChange> Santa_GiftingGroup_AuditChanges => Set<Santa_GiftingGroup_AuditChange>();
    public DbSet<Santa_GiftingGroupApplication> Santa_GiftingGroupApplications => Set<Santa_GiftingGroupApplication>();
    public DbSet<Santa_GiftingGroupUser> Santa_GiftingGroupUsers => Set<Santa_GiftingGroupUser>();
    public DbSet<Santa_GiftingGroupYear> Santa_GiftingGroupYears => Set<Santa_GiftingGroupYear>();
    public DbSet<Santa_GiftingGroupYear_Audit> Santa_GiftingGroupYear_Audit => Set<Santa_GiftingGroupYear_Audit>();
    public DbSet<Santa_GiftingGroupYear_AuditChange> Santa_GiftingGroupYear_AuditChanges => Set<Santa_GiftingGroupYear_AuditChange>();
    public DbSet<Santa_Message> Santa_Messages => Set<Santa_Message>();
    public DbSet<Santa_MessageRecipient> Santa_MessageRecipients => Set<Santa_MessageRecipient>();
    public DbSet<Santa_MessageReply> Santa_MessageReplies => Set<Santa_MessageReply>();
    public DbSet<Santa_PartnerLink> Santa_PartnerLinks => Set<Santa_PartnerLink>();
    public DbSet<Santa_Suggestion> Santa_Suggestions => Set<Santa_Suggestion>();
    public DbSet<Santa_User> Santa_Users => Set<Santa_User>();
    public DbSet<Santa_YearGroupUser> Santa_YearGroupUsers => Set<Santa_YearGroupUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ModelHelper.OnModelCreating(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .AddUserSecrets(Assembly.GetExecutingAssembly())
                .Build();

            var connectionStringBuilder = new SqlConnectionStringBuilder(configuration.GetConnectionString("DefaultConnection"));
            connectionStringBuilder.UserID = configuration["DatabaseSettings:DevUserId"];
            connectionStringBuilder.Password = configuration["DatabaseSettings:DevPassword"];
            string connectionString = connectionStringBuilder.ConnectionString;

            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer(connectionString);
        }
    }
}