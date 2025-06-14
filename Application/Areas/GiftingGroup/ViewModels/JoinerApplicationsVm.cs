using Global.Abstractions.Areas.GiftingGroup;

namespace Application.Areas.GiftingGroup.ViewModels;

public sealed class JoinerApplicationsVm
{
    public IQueryable<IReviewApplication> Applications { get; set; } = new List<IReviewApplication>().AsQueryable();
    public int ApplicationsCount => Applications.Count();
}
