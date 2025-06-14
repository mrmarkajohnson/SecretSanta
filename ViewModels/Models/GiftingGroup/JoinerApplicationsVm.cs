using Global.Abstractions.Areas.GiftingGroup;

namespace ViewModels.Models.GiftingGroup;

public sealed class JoinerApplicationsVm
{
    public IQueryable<IReviewApplication> Applications { get; set; } = new List<IReviewApplication>().AsQueryable();
    public int ApplicationsCount => Applications.Count();
}
