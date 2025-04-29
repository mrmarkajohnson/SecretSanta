namespace Global.Abstractions.Areas.GiftingGroup;

public interface IUserGroupYearShared
{
    string GiftingGroupName { get; }

    /// <summary>
    /// Is the user participating in this group for this year?
    /// </summary>
    bool Included { get; set; }
}
