namespace Global.Abstractions.Santa.Areas.GiftingGroup;

public interface IUserGiftingGroup
{
    int GiftingGroupId { get; }
    string GroupName { get; }    
    bool GroupAdmin { get; }
    int NewApplications { get; }
}
