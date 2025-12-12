using ROGraph.NewUI.ViewModels;

namespace ROGraph.NewUI.Dialogs.DeleteConfirmDialog;

public class DeleteConfirmDialogViewControl : ViewModelBase
{
    public string DisplayName { get; set; }

    public DeleteConfirmDialogViewControl(string name)
    {
        this.DisplayName = name;
    }
}