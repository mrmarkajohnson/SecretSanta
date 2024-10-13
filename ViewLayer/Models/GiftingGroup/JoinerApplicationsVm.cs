using Global.Abstractions.Santa.Areas.GiftingGroup;

namespace ViewLayer.Models.GiftingGroup;

public class JoinerApplicationsVm
{
    public IQueryable<IReviewApplication> Applications { get; set; } = new List<IReviewApplication>().AsQueryable();
    public int ApplicationsCount => Applications.Count();
}
