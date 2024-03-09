using Data.Entities.Data.Global;
using Data.Entities.Data.Santa;
using Microsoft.EntityFrameworkCore;

namespace Data.Helpers;

internal static class ModelHelper
{
    public static void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Global_User>()
            .HasOne(e => e.SantaUser)
            .WithOne(e => e.GlobalUser)
            .HasForeignKey<Santa_User>(e => e.GlobalUserId)
            .IsRequired(false);

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

        modelBuilder.Entity<Santa_GiftingGroupYear>()
            .HasOne(e => e.GiftingGroup)
            .WithMany(e => e.Years)
            .HasForeignKey(e => e.GiftingGroupId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Santa_GiftingGroupUser>()
            .HasOne(e => e.GiftingGroup)
            .WithMany(e => e.UserLinks)
            .HasForeignKey(e => e.GiftingGroupId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Santa_GiftingGroupUser>()
            .HasOne(e => e.User)
            .WithMany(e => e.GiftingGroupLinks)
            .HasForeignKey(e => e.GiftingGroupId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Santa_PartnerLink>()
            .HasOne(e => e.Partner1)
            .WithMany(e => e.Partner1Links)
            .HasForeignKey(e => e.Partner1Id)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Santa_PartnerLink>()
            .HasOne(e => e.Partner2)
            .WithMany(e => e.Partner2Links)
            .HasForeignKey(e => e.Partner2Id)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
