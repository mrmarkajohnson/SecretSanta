namespace Application.Areas.GiftingGroup.Queries;

public sealed class GiftingGroupsCountQuery : BaseQuery<int>
{
    protected override Task<int> Handle()
    {
        return Task.FromResult(DbContext.Santa_GiftingGroups
            .Count(GroupExpressions.IsActive()));
    }
}