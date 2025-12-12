using ROGraph.NewUI.ViewModels;

namespace ROGraph.NewUI.Dialogs.DeleteConfirmDialog;

public class DeleteConfirmDialogViewModel : ViewModelBase
{
    public string DisplayName { get; set; }

    public DeleteConfirmDialogViewModel(string name)
    {
        this.DisplayName = name;
    }
}