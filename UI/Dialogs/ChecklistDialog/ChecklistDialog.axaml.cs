using Avalonia.Controls;
using ROGraph.Shared.Models.Checklist;

namespace ROGraph.UI.Dialogs.ChecklistDialog;

internal partial class ChecklistDialog : Window
{
    public ChecklistDialog(Checklist checklist)
    {
        InitializeComponent();
        
        DataContext = new ChecklistDialogViewModel(checklist);
    }
}