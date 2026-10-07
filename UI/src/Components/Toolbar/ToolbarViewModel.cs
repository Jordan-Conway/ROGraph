using System.Collections.Generic;
using System.Collections.ObjectModel;
using ROGraph.UI.ViewModels;

namespace ROGraph.UI.Components.Toolbar;

internal partial class ToolbarViewModel : ViewModelBase
{
    public ToolbarContent Content { get; init; }

    public ToolbarViewModel(ToolbarContent content)
    {
        Content = content;
    }
}