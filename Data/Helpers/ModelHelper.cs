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
            .HasForeignKey(e => e.AuditId);

        modelBuilder.Entity<Santa_GiftingGroup>()
            .HasMany(e => e.AuditTrail)
            .WithOne(e => e.Parent)
            .HasForeignKey(e => e.ParentId);

        modelBuilder.Entity<Santa_GiftingGroup_Audit>()
            .HasMany(e => e.Changes)
            .WithOne(e => e.Audit)
            .HasForeignKey(e => e.AuditId);

        modelBuilder.Entity<Santa_GiftingGroupApplication>()
            .HasOne(e => e.SantaUser)
            .WithMany(e => e.GiftingGroupApplications)
            .HasForeignKey(e => e.SantaUserId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Santa_GiftingGroupApplication>()
            .HasOne(e => e.GiftingGroup)
            .WithMany(e => e.MemberApplications)
            .HasForeignKey(e => e.GiftingGroupId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Santa_GiftingGroupApplication>()
            .HasOne(e => e.ResponseByUser)
            .WithMany(e => e.GiftingGroupApplicationResponses)
            .HasForeignKey(e => e.ResponseByUserId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Santa_GiftingGroupUser>()
            .HasOne(e => e.GiftingGroup)
            .WithMany(e => e.UserLinks)
            .HasForeignKey(e => e.GiftingGroupId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Santa_GiftingGroupUser>()
            .HasOne(e => e.SantaUser)
            .WithMany(e => e.GiftingGroupLinks)
            .HasForeignKey(e => e.SantaUserId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Santa_GiftingGroupYear>()
            .HasOne(e => e.GiftingGroup)
            .WithMany(e => e.Years)
            .HasForeignKey(e => e.GiftingGroupId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Santa_GiftingGroupYear_Audit>()
            .HasMany(e => e.Changes)
            .WithOne(e => e.Audit)
            .HasForeignKey(e => e.AuditId);

        modelBuilder.Entity<Santa_PartnerLink>()
            .HasOne(e => e.SuggestedBySantaUser)
            .WithMany(e => e.SuggestedRelationships)
            .HasForeignKey(e => e.SuggestedBySantaUserId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Santa_PartnerLink>()
            .HasOne(e => e.ConfirmingSantaUser)
            .WithMany(e => e.ConfirmingRelationships)
            .HasForeignKey(e => e.ConfirmingSantaUserId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Santa_Message>()
            .HasOne(e => e.GiftingGroupYear)
            .WithMany(e => e.Messages)
            .HasForeignKey(e => e.GiftingGroupYearId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Santa_Message>()
            .HasOne(e => e.Sender)
            .WithMany(e => e.SentMessages) 
            .HasForeignKey(e => e.SenderId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Santa_Message>()
            .HasOne(e => e.ReplyTo)
            .WithOne(e => e.ReplyMessage)
            .HasForeignKey<Santa_MessageReply>(e => e.ReplyMessageId)
            .IsRequired(false);

        modelBuilder.Entity<Santa_MessageReply>()
            .HasOne(e => e.OriginalMessage)
            .WithMany(e => e.Replies)
            .HasForeignKey(e => e.OriginalMessageId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Santa_MessageRecipient>()
            .HasOne(e => e.Message)
            .WithMany(e => e.Recipients)
            .HasForeignKey(e => e.MessageId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Santa_MessageRecipient>()
            .HasOne(e => e.Recipient)
            .WithMany(e => e.ReceivedMessages)
            .HasForeignKey(e => e.RecipientId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Santa_Suggestion>()
            .HasOne(e => e.Suggester)
            .WithMany(e => e.Suggestions)
            .HasForeignKey(e => e.SuggesterId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Santa_YearGroupUser>()
            .HasOne(e => e.Year)
            .WithMany(e => e.Users)
            .HasForeignKey(e => e.YearId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Santa_YearGroupUser>()
            .HasOne(e => e.GivingToUser)
            .WithMany(e => e.GiftingGroupYears)
            .HasForeignKey(e => e.GivingToUserId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
