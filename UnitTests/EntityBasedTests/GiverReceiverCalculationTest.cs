using Application.Areas.GiftingGroup.BaseModels;
using Application.Areas.GiftingGroup.Queries.Internal;
using Data.Entities.Santa;
using Data.Entities.Shared;
using Microsoft.Extensions.DependencyInjection;
using UnitTests.Context;
using Xunit;

namespace UnitTests.EntityBasedTests;

public sealed class GiverReceiverCalculationTest : EntityBasedTestBase
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
                    participatingMembers.First(x => x.SantaUserKey == combi.GiverSantaUserKey).RecipientSantaUserKey = combi.RecipientSantaUserKey;
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
        Assert.True(participatingMembers.All(x => x.RecipientSantaUserKey != null));
    }

    private static void EnsureNobodyGivingToThemself(List<Santa_YearGroupUser> participatingMembers)
    {
        Assert.DoesNotContain(participatingMembers, x => x.RecipientSantaUserKey == x.SantaUserKey);
    }

    private static void EnsureNoDuplication(List<Santa_YearGroupUser> participatingMembers)
    {
        Assert.DoesNotContain(participatingMembers.GroupBy(x => x.RecipientSantaUserKey), y => y.Count() > 1);
    }

    private static void EnsureNobodyIsGivingToAPartner(List<Santa_YearGroupUser> participatingMembers)
    {
        Assert.DoesNotContain(participatingMembers, x => x.SantaUser.SuggestedRelationships
            .Where(y => y.RelationshipEnded == null || (y.SuggestedByIgnoreOld && y.ConfirmingUserIgnore))
            .Any(y => y.ConfirmingSantaUserKey == x.RecipientSantaUserKey));

        Assert.DoesNotContain(participatingMembers, x => x.SantaUser.ConfirmingRelationships
            .Where(y => y.RelationshipEnded == null || (y.SuggestedByIgnoreOld && y.ConfirmingUserIgnore))
            .Any(y => y.SuggestedBySantaUserKey == x.RecipientSantaUserKey));
    }

    private static void TestPreviousYearDuplicates(List<Santa_YearGroupUser> participatingMembers, Santa_GiftingGroupYear dbYear, int actualPreviousYears)
    {
        var dbPreviousYears = dbYear.GiftingGroup.Years.Where(x => x.CalendarYear < dbYear.CalendarYear && x.CalendarYear >= dbYear.CalendarYear - actualPreviousYears);

        Assert.DoesNotContain(participatingMembers, x => dbPreviousYears
            .Any(y => y.Users
                .Where(z => z.SantaUserKey == x.SantaUserKey)
                .Any(z => z.RecipientSantaUserKey == x.RecipientSantaUserKey)));
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
            SantaUserKey = 1,
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
            SantaUserKey = 2,
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
            SantaUserKey = 3,
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
            SantaUserKey = 4,
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
            SantaUserKey = 5,
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
            SantaUserKey = 6,
            GlobalUserId = user6.Id,
            GlobalUser = user6
        };

        context.Users.Add(user6);

        #endregion Santa Users

        #region Relationships

        var relationship1 = new Santa_PartnerLink
        {
            PartnerLinkKey = 1,
            SuggestedBySantaUserKey = 1,
            SuggestedBySantaUser = santaUser1,
            ConfirmingSantaUserKey = 2,
            ConfirmingSantaUser = santaUser2,
            Confirmed = true,
            ExchangeGifts = false
        };

        santaUser1.SuggestedRelationships.Add(relationship1);
        santaUser2.SuggestedRelationships.Add(relationship1);

        var relationship2 = new Santa_PartnerLink
        {
            PartnerLinkKey = 2,
            SuggestedBySantaUserKey = 3,
            SuggestedBySantaUser = santaUser3,
            ConfirmingSantaUserKey = 4,
            ConfirmingSantaUser = santaUser4,
            Confirmed = true,
            RelationshipEnded = DateTime.Today.AddYears(-1), // ended but still counts
            ExchangeGifts = false
        };

        santaUser3.SuggestedRelationships.Add(relationship2);
        santaUser4.SuggestedRelationships.Add(relationship2);

        var relationship3 = new Santa_PartnerLink
        {
            PartnerLinkKey = 3,
            SuggestedBySantaUserKey = 5,
            SuggestedBySantaUser = santaUser5,
            ConfirmingSantaUserKey = 6,
            ConfirmingSantaUser = santaUser6,
            Confirmed = true,
            RelationshipEnded = DateTime.Today.AddYears(-1),
            SuggestedByIgnoreOld = true,
            ConfirmingUserIgnore = true, // ended and doesn't count any more
            ExchangeGifts = true
        };

        santaUser5.SuggestedRelationships.Add(relationship3);
        santaUser6.SuggestedRelationships.Add(relationship3);

        #endregion Relationships

        #region Santa Groups

        var giftingGroup1 = new Santa_GiftingGroup
        {
            GiftingGroupKey = 1,
            Name = "GG1",
            Description = "GG1",
            JoinerToken = "123456"
        };

        context.Santa_GiftingGroups.Add(giftingGroup1);

        var userLink1 = new Santa_GiftingGroupUser
        {
            GiftingGroupUserKey = 1,
            GroupAdmin = true,
            SantaUserKey = 1,
            SantaUser = santaUser1,
            GiftingGroupKey = 1,
            GiftingGroup = giftingGroup1
        };

        giftingGroup1.Members.Add(userLink1);

        var userLink2 = new Santa_GiftingGroupUser
        {
            GiftingGroupUserKey = 2,
            GroupAdmin = true,
            SantaUserKey = 2,
            SantaUser = santaUser2,
            GiftingGroupKey = 1,
            GiftingGroup = giftingGroup1
        };

        giftingGroup1.Members.Add(userLink2);

        var userLink3 = new Santa_GiftingGroupUser
        {
            GiftingGroupUserKey = 3,
            GroupAdmin = true,
            SantaUserKey = 3,
            SantaUser = santaUser3,
            GiftingGroupKey = 1,
            GiftingGroup = giftingGroup1
        };

        giftingGroup1.Members.Add(userLink3);

        var userLink4 = new Santa_GiftingGroupUser
        {
            GiftingGroupUserKey = 4,
            GroupAdmin = true,
            SantaUserKey = 4,
            SantaUser = santaUser4,
            GiftingGroupKey = 1,
            GiftingGroup = giftingGroup1
        };

        giftingGroup1.Members.Add(userLink4);

        var userLink5 = new Santa_GiftingGroupUser
        {
            GiftingGroupUserKey = 5,
            GroupAdmin = true,
            SantaUserKey = 5,
            SantaUser = santaUser5,
            GiftingGroupKey = 1,
            GiftingGroup = giftingGroup1
        };

        giftingGroup1.Members.Add(userLink5);

        var userLink6 = new Santa_GiftingGroupUser
        {
            GiftingGroupUserKey = 6,
            GroupAdmin = true,
            SantaUserKey = 6,
            SantaUser = santaUser6,
            GiftingGroupKey = 1,
            GiftingGroup = giftingGroup1
        };

        giftingGroup1.Members.Add(userLink6);

        #endregion Santa Groups

        #region Group Years

        int groupYearKey = 0;
        int yearGroupUserKey = 0;

        List<int> years = [2022, 2023, 2024];

        foreach (int year in years)
        {
            var giftingYear1 = new Santa_GiftingGroupYear
            {
                GiftingGroupYearKey = ++groupYearKey,
                CalendarYear = year,
                Limit = 50,
                GiftingGroupKey = 1,
                GiftingGroup = giftingGroup1,
                CurrencyCode = "GBP",
                CurrencySymbol = "£"
            };

            context.Santa_GiftingGroupYears.Add(giftingYear1);

            foreach (var userLink in giftingGroup1.Members)
            {
                var yearUser = new Santa_YearGroupUser
                {
                    YearGroupUserKey = ++yearGroupUserKey,
                    GiftingGroupYearKey = groupYearKey,
                    GiftingGroupYear = giftingYear1,
                    SantaUserKey = userLink.SantaUserKey,
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
