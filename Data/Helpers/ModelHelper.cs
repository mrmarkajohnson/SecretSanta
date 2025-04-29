using Data.Entities.Santa;
using Data.Entities.Shared;
using Microsoft.EntityFrameworkCore;

namespace Data.Helpers;

internal static class ModelHelper
{
    public static void Configure<TEntity>(TEntity entity, ModelBuilder modelBuilder) where TEntity : class, IEntity
    {
        modelBuilder.Entity<TEntity>().UseTpcMappingStrategy();
    }

    public static void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (IEntity e in modelBuilder.Model.GetEntityTypes().OfType<IEntity>())
        {
            Configure(e, modelBuilder);
        }

        modelBuilder.Entity<Global_User>()
            .HasOne(e => e.SantaUser)
            .WithOne(e => e.GlobalUser)
            .HasForeignKey<Santa_User>(e => e.GlobalUserId)
            .IsRequired(false);

        modelBuilder.Entity<Global_User>()
            .HasMany(e => e.AuditTrail)
            .WithOne(e => e.Parent)
            .HasForeignKey(e => e.ParentId);

        modelBuilder.Entity<Global_User_Audit>()
            .HasMany(e => e.Changes)
            .WithOne(e => e.Audit)
            .HasForeignKey(e => e.AuditKey);

        modelBuilder.Entity<Santa_GiftingGroup>()
            .HasMany(e => e.AuditTrail)
            .WithOne(e => e.Parent)
            .HasForeignKey(e => e.ParentKey);

        modelBuilder.Entity<Santa_GiftingGroup>()
            .HasMany(e => e.AuditTrail)
            .WithOne(e => e.Parent)
            .HasForeignKey(e => e.ParentKey);

        modelBuilder.Entity<Santa_GiftingGroup_Audit>()
            .HasMany(e => e.Changes)
            .WithOne(e => e.Audit)
            .HasForeignKey(e => e.AuditKey);

        modelBuilder.Entity<Santa_GiftingGroupApplication>()
            .HasOne(e => e.SantaUser)
            .WithMany(e => e.GiftingGroupApplications)
            .HasForeignKey(e => e.SantaUserKey)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Santa_GiftingGroupApplication>()
            .HasOne(e => e.GiftingGroup)
            .WithMany(e => e.MemberApplications)
            .HasForeignKey(e => e.GiftingGroupKey)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Santa_GiftingGroupApplication>()
            .HasOne(e => e.ResponseBySantaUser)
            .WithMany(e => e.GiftingGroupApplicationResponses)
            .HasForeignKey(e => e.ResponseBySantaUserKey)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Santa_GiftingGroupUser>()
            .HasOne(e => e.GiftingGroup)
            .WithMany(e => e.UserLinks)
            .HasForeignKey(e => e.GiftingGroupKey)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Santa_GiftingGroupUser>()
            .HasOne(e => e.SantaUser)
            .WithMany(e => e.GiftingGroupLinks)
            .HasForeignKey(e => e.SantaUserKey)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Santa_GiftingGroupYear>()
            .HasOne(e => e.GiftingGroup)
            .WithMany(e => e.Years)
            .HasForeignKey(e => e.GiftingGroupKey)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Santa_GiftingGroupYear>()
            .HasMany(e => e.AuditTrail)
            .WithOne(e => e.Parent)
            .HasForeignKey(e => e.ParentKey);

        modelBuilder.Entity<Santa_GiftingGroupYear_Audit>()
            .HasMany(e => e.Changes)
            .WithOne(e => e.Audit)
            .HasForeignKey(e => e.AuditKey);

        modelBuilder.Entity<Santa_PartnerLink>()
            .HasOne(e => e.SuggestedBySantaUser)
            .WithMany(e => e.SuggestedRelationships)
            .HasForeignKey(e => e.SuggestedBySantaUserKey)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Santa_PartnerLink>()
            .HasOne(e => e.ConfirmingSantaUser)
            .WithMany(e => e.ConfirmingRelationships)
            .HasForeignKey(e => e.ConfirmingSantaUserKey)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Santa_Message>()
            .HasOne(e => e.GiftingGroupYear)
            .WithMany(e => e.Messages)
            .HasForeignKey(e => e.GiftingGroupYearKey)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Santa_Message>()
            .HasOne(e => e.Sender)
            .WithMany(e => e.SentMessages)
            .HasForeignKey(e => e.SenderKey)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Santa_Message>()
            .HasOne(e => e.ReplyTo)
            .WithOne(e => e.ReplyMessage)
            .HasForeignKey<Santa_MessageReply>(e => e.ReplyMessageKey)
            .IsRequired(false);

        modelBuilder.Entity<Santa_MessageReply>()
            .HasOne(e => e.OriginalMessage)
            .WithMany(e => e.Replies)
            .HasForeignKey(e => e.OriginalMessageKey)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Santa_MessageRecipient>()
            .HasOne(e => e.Message)
            .WithMany(e => e.Recipients)
            .HasForeignKey(e => e.MessageKey)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Santa_MessageRecipient>()
            .HasOne(e => e.RecipientSantaUser)
            .WithMany(e => e.ReceivedMessages)
            .HasForeignKey(e => e.RecipientSantaUserKey)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Santa_Suggestion>()
            .HasOne(e => e.SantaUser)
            .WithMany(e => e.Suggestions)
            .HasForeignKey(e => e.SantaUserKey)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Santa_Suggestion>()
            .HasMany(e => e.AuditTrail)
            .WithOne(e => e.Parent)
            .HasForeignKey(e => e.ParentKey);

        modelBuilder.Entity<Santa_Suggestion_Audit>()
            .HasMany(e => e.Changes)
            .WithOne(e => e.Audit)
            .HasForeignKey(e => e.AuditKey);

        modelBuilder.Entity<Santa_SuggestionLink>()
            .HasOne(e => e.Suggestion)
            .WithMany(e => e.YearGroupUserLinks)
            .HasForeignKey(e => e.SuggestionKey)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Santa_SuggestionLink>()
            .HasOne(e => e.YearGroupUser)
            .WithMany(e => e.Suggestions)
            .HasForeignKey(e => e.YearGroupUserKey)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Santa_SuggestionLink>()
            .HasMany(e => e.AuditTrail)
            .WithOne(e => e.Parent)
            .HasForeignKey(e => e.ParentKey);

        modelBuilder.Entity<Santa_SuggestionLink_Audit>()
            .HasMany(e => e.Changes)
            .WithOne(e => e.Audit)
            .HasForeignKey(e => e.AuditKey);

        modelBuilder.Entity<Santa_YearGroupUser>()
            .HasOne(e => e.GiftingGroupYear)
            .WithMany(e => e.Users)
            .HasForeignKey(e => e.GiftingGroupYearKey)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Santa_YearGroupUser>()
            .HasOne(e => e.SantaUser)
            .WithMany(e => e.GiftingGroupYears)
            .HasForeignKey(e => e.SantaUserKey)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Santa_YearGroupUser>()
            .HasOne(e => e.RecipientSantaUser)
            .WithMany(e => e.RecipientYears)
            .HasForeignKey(e => e.RecipientSantaUserKey)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Santa_YearGroupUser>()
            .HasMany(e => e.AuditTrail)
            .WithOne(e => e.Parent)
            .HasForeignKey(e => e.ParentKey);

        modelBuilder.Entity<Santa_YearGroupUser_Audit>()
            .HasMany(e => e.Changes)
            .WithOne(e => e.Audit)
            .HasForeignKey(e => e.AuditKey);
    }
}
