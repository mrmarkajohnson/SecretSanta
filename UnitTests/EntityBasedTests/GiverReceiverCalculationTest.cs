using Application.Areas.GiftingGroup.BaseModels;
using Application.Areas.GiftingGroup.Queries.Internal;
using Data.Entities.Santa;
using Data.Entities.Shared;
using Microsoft.Extensions.DependencyInjection;
using UnitTests.Context;
using Xunit;

namespace UnitTests.EntityBasedTests;

public class GiverReceiverCalculationTest : EntityBasedTestBase
{
    [Fact]
    public async Task TestGiverReceiverCalculation()
    {
        var context = new TestDbContext();
        await SeedContext(context);
        ServiceProvider serviceProvider = GetServiceProvider(context);
        
        // TODO: Use CreateSantaUserCommand or similar to set up and sign in CurrentUser when needed?

        foreach (var dbYear in context.Santa_GiftingGroupYears.ToList())
        {
            int targetPreviousYears = 2;
            int actualPreviousYears = targetPreviousYears;
            var query = new CalculateGiversAndReceiversQuery(dbYear, ref actualPreviousYears);
            query.ClaimsUserNotRequired = true;
            var yearResult = await query.Handle(serviceProvider, CurrentUser);

            AssureResultsCalculated(yearResult);
            EnsureTargetPreviousYearsMet(targetPreviousYears, actualPreviousYears);

            if (yearResult.Count > 0)
            {
                List<Santa_YearGroupUser> participatingMembers = dbYear.ParticipatingMembers();
                Assert.True(participatingMembers.Any());

                foreach (GiverAndReceiverCombination combi in yearResult)
                {
                    participatingMembers.First(x => x.SantaUserId == combi.GiverId).GivingToUserId = combi.RecipientId;
                }

                context.ChangeTracker.DetectChanges();

                EnsureEveryoneHasARecipient(participatingMembers);
                EnsureNobodyGivingToThemself(participatingMembers);
                EnsureNoDuplication(participatingMembers);
                EnsureNobodyIsGivingToAPartner(participatingMembers);
                TestPreviousYearDuplicates(participatingMembers, dbYear, actualPreviousYears);

                // TODO: Look into whether any previous relationships are involved
            }
        }
    }

    private static void AssureResultsCalculated(List<GiverAndReceiverCombination> yearResult)
    {
        Assert.True(yearResult.Any());
    }

    private static void EnsureTargetPreviousYearsMet(int targetPreviousYears, int actualPreviousYears)
    {
        Assert.Equal(targetPreviousYears, actualPreviousYears);
    }

    private static void EnsureEveryoneHasARecipient(List<Santa_YearGroupUser> participatingMembers)
    {
        Assert.True(participatingMembers.All(x => x.GivingToUserId != null));
    }

    private static void EnsureNobodyGivingToThemself(List<Santa_YearGroupUser> participatingMembers)
    {
        Assert.DoesNotContain(participatingMembers, x => x.GivingToUserId == x.SantaUserId);
    }

    private static void EnsureNoDuplication(List<Santa_YearGroupUser> participatingMembers)
    {
        Assert.DoesNotContain(participatingMembers.GroupBy(x => x.GivingToUserId), y => y.Count() > 1);
    }

    private static void EnsureNobodyIsGivingToAPartner(List<Santa_YearGroupUser> participatingMembers)
    {
        Assert.DoesNotContain(participatingMembers, x => x.SantaUser.SuggestedRelationships
            .Where(y => y.RelationshipEnded == null || (y.SuggestedByIgnoreOld && y.ConfirmedByIgnoreOld))
            .Any(y => y.ConfirmingSantaUserId == x.GivingToUserId));

        Assert.DoesNotContain(participatingMembers, x => x.SantaUser.ConfirmingRelationships
            .Where(y => y.RelationshipEnded == null || (y.SuggestedByIgnoreOld && y.ConfirmedByIgnoreOld))
            .Any(y => y.SuggestedBySantaUserId == x.GivingToUserId));
    }

    private static void TestPreviousYearDuplicates(List<Santa_YearGroupUser> participatingMembers, Santa_GiftingGroupYear dbYear, int actualPreviousYears)
    {
        var dbPreviousYears = dbYear.GiftingGroup.Years.Where(x => x.Year < dbYear.Year && x.Year >= dbYear.Year - actualPreviousYears);

        Assert.DoesNotContain(participatingMembers, x => dbPreviousYears
            .Any(y => y.Users
                .Where(z => z.SantaUserId == x.SantaUserId)
                .Any(z => z.GivingToUserId == x.GivingToUserId)));
    }

    private static async Task SeedContext(TestDbContext context)
    {
        #region Santa Users

        var user1 = new Global_User
        {
            Id = Guid.NewGuid().ToString(),
            Forename = "A",
            Surname = "B",
            Greeting = ""
        };

        var santaUser1 = user1.SantaUser = new Santa_User
        {
            Id = 1,
            GlobalUserId = user1.Id,
            GlobalUser = user1
        };

        context.Users.Add(user1);

        var user2 = new Global_User
        {
            Id = Guid.NewGuid().ToString(),
            Forename = "C",
            Surname = "D",
            Greeting = ""
        };

        var santaUser2 = user2.SantaUser = new Santa_User
        {
            Id = 2,
            GlobalUserId = user2.Id,
            GlobalUser = user2
        };

        context.Users.Add(user2);

        var user3 = new Global_User
        {
            Id = Guid.NewGuid().ToString(),
            Forename = "E",
            Surname = "F",
            Greeting = ""
        };

        var santaUser3 = user3.SantaUser = new Santa_User
        {
            Id = 3,
            GlobalUserId = user3.Id,
            GlobalUser = user3
        };

        context.Users.Add(user3);

        var user4 = new Global_User
        {
            Id = Guid.NewGuid().ToString(),
            Forename = "G",
            Surname = "H",
            Greeting = ""
        };

        var santaUser4 = user4.SantaUser = new Santa_User
        {
            Id = 4,
            GlobalUserId = user4.Id,
            GlobalUser = user4
        };

        context.Users.Add(user4);

        var user5 = new Global_User
        {
            Id = Guid.NewGuid().ToString(),
            Forename = "I",
            Surname = "J",
            Greeting = ""
        };

        var santaUser5 = user5.SantaUser = new Santa_User
        {
            Id = 5,
            GlobalUserId = user5.Id,
            GlobalUser = user5
        };

        context.Users.Add(user5);

        var user6 = new Global_User
        {
            Id = Guid.NewGuid().ToString(),
            Forename = "K",
            Surname = "L",
            Greeting = ""
        };

        var santaUser6 = user6.SantaUser = new Santa_User
        {
            Id = 6,
            GlobalUserId = user6.Id,
            GlobalUser = user6
        };

        context.Users.Add(user6);

        #endregion Santa Users

        #region Relationships

        var relationship1 = new Santa_PartnerLink
        {
            Id = 1,
            SuggestedBySantaUserId = 1,
            SuggestedBySantaUser = santaUser1,
            ConfirmingSantaUserId = 2,
            ConfirmingSantaUser = santaUser2,
            Confirmed = true
        };

        santaUser1.SuggestedRelationships.Add(relationship1);
        santaUser2.SuggestedRelationships.Add(relationship1);

        var relationship2 = new Santa_PartnerLink
        {
            Id = 2,
            SuggestedBySantaUserId = 3,
            SuggestedBySantaUser = santaUser3,
            ConfirmingSantaUserId = 4,
            ConfirmingSantaUser = santaUser4,
            Confirmed = true,
            RelationshipEnded = DateTime.Today.AddYears(-1) // ended but still counts
        };

        santaUser3.SuggestedRelationships.Add(relationship2);
        santaUser4.SuggestedRelationships.Add(relationship2);

        var relationship3 = new Santa_PartnerLink
        {
            Id = 3,
            SuggestedBySantaUserId = 5,
            SuggestedBySantaUser = santaUser5,
            ConfirmingSantaUserId = 6,
            ConfirmingSantaUser = santaUser6,
            Confirmed = true,
            RelationshipEnded = DateTime.Today.AddYears(-1),
            SuggestedByIgnoreOld = true,
            ConfirmedByIgnoreOld = true // ended and doesn't count any more
        };

        santaUser5.SuggestedRelationships.Add(relationship3);
        santaUser6.SuggestedRelationships.Add(relationship3);

        #endregion Relationships

        #region Santa Groups

        var giftingGroup1 = new Santa_GiftingGroup
        {
            Id = 1,
            Name = "GG1",
            Description = "GG1",
            JoinerToken = "123456"
        };

        context.Santa_GiftingGroups.Add(giftingGroup1);

        var userLink1 = new Santa_GiftingGroupUser
        {
            Id = 1,
            GroupAdmin = true,
            SantaUserId = 1,
            SantaUser = santaUser1,
            GiftingGroupId = 1,
            GiftingGroup = giftingGroup1
        };

        giftingGroup1.UserLinks.Add(userLink1);

        var userLink2 = new Santa_GiftingGroupUser
        {
            Id = 2,
            GroupAdmin = true,
            SantaUserId = 2,
            SantaUser = santaUser2,
            GiftingGroupId = 1,
            GiftingGroup = giftingGroup1
        };

        giftingGroup1.UserLinks.Add(userLink2);

        var userLink3 = new Santa_GiftingGroupUser
        {
            Id = 3,
            GroupAdmin = true,
            SantaUserId = 3,
            SantaUser = santaUser3,
            GiftingGroupId = 1,
            GiftingGroup = giftingGroup1
        };

        giftingGroup1.UserLinks.Add(userLink3);

        var userLink4 = new Santa_GiftingGroupUser
        {
            Id = 4,
            GroupAdmin = true,
            SantaUserId = 4,
            SantaUser = santaUser4,
            GiftingGroupId = 1,
            GiftingGroup = giftingGroup1
        };

        giftingGroup1.UserLinks.Add(userLink4);

        var userLink5 = new Santa_GiftingGroupUser
        {
            Id = 5,
            GroupAdmin = true,
            SantaUserId = 5,
            SantaUser = santaUser5,
            GiftingGroupId = 1,
            GiftingGroup = giftingGroup1
        };

        giftingGroup1.UserLinks.Add(userLink5);

        var userLink6 = new Santa_GiftingGroupUser
        {
            Id = 6,
            GroupAdmin = true,
            SantaUserId = 6,
            SantaUser = santaUser6,
            GiftingGroupId = 1,
            GiftingGroup = giftingGroup1
        };

        giftingGroup1.UserLinks.Add(userLink6);

        #endregion Santa Groups

        #region Group Years

        int yearId = 0;
        int groupUserId = 0;

        List<int> years = [2022, 2023, 2024];

        foreach (int year in years)
        {
            var giftingYear1 = new Santa_GiftingGroupYear
            {
                Id = ++yearId,
                Year = year,
                Limit = 50,
                GiftingGroupId = 1,
                GiftingGroup = giftingGroup1
            };

            context.Santa_GiftingGroupYears.Add(giftingYear1);

            foreach (var userLink in giftingGroup1.UserLinks)
            {
                var yearUser = new Santa_YearGroupUser
                {
                    Id = ++groupUserId,
                    YearId = yearId,
                    Year = giftingYear1,
                    SantaUserId = userLink.SantaUserId,
                    SantaUser = userLink.SantaUser,
                    Included = true
                };

                giftingYear1.Users.Add(yearUser);
            }
        }

        #endregion Group Years

        context.ChangeTracker.DetectChanges();

        await context.SaveChangesAsync();
    }
}
