namespace Application.Areas.GiftingGroup.Queries;

public class GiftingGroupsCountQuery : BaseQuery<int>
{
    protected override Task<int> Handle()
    {
        return Task.FromResult(DbContext.Santa_GiftingGroups.Where(x => x.DateDeleted == null).Count());
    }
}