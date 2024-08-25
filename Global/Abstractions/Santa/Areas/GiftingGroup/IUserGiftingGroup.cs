namespace Global.Abstractions.Santa.Areas.GiftingGroup;

public interface IUserGiftingGroup
{
    int GroupId { get; }
    string GroupName { get; }    
    bool GroupAdmin { get; }
}
