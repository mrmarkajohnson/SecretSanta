namespace Application.Shared.BaseModels;

internal class VisibleUser : UserNamesBase, IVisibleUser
{
    public VisibleUser()
    {
        SharedGroupNames = new List<string>();
    }

    public IList<string> SharedGroupNames { get; }
}
