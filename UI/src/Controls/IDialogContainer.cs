using CommunityToolkit.Mvvm.ComponentModel;
using ROGraph.UI.Dialogs;

namespace ROGraph.UI.Controls;

public interface IDialogContainer
{
    BaseDialog? DialogControl
    {
        get;
        set;
    }
}