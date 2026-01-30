namespace AgNext.UI.ViewModels;

using System.Collections.ObjectModel;

public sealed class SectionStatusModuleViewModel : UiModuleViewModel
{
    public ObservableCollection<RowStatusViewModel> Rows { get; } = new();

    public SectionStatusModuleViewModel(string id, string title)
        : base(id, title, UiModuleRegion.RightRail, isMovable: false)
    {
    }
}
