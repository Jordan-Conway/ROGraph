using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ROGraph.UI.Components.Toolbar;

internal class ToolbarContent
{
    public ObservableCollection<ToolbarItem> LeftItems { get; init; }

    public ObservableCollection<ToolbarItem> MiddleItems { get; init; }

    public ObservableCollection<ToolbarItem> RightItems { get; init; }
    
    public ToolbarContent(
        IEnumerable<ToolbarItem>? leftItems,
        IEnumerable<ToolbarItem>? middleItems,
        IEnumerable<ToolbarItem>? rightItems
        )
    {
        LeftItems = new ObservableCollection<ToolbarItem>(leftItems ?? []);
        MiddleItems = new ObservableCollection<ToolbarItem>(middleItems ?? []);
        RightItems = new ObservableCollection<ToolbarItem>(rightItems ?? []);
    }
}