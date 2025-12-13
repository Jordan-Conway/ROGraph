using ROGraph.NewUI.ViewModels;

namespace ROGraph.NewUI.Dialogs.ConfirmDialog;

public class ConfirmDialogViewModel : ViewModelBase
{
    public string Message { get; set; }

    public ConfirmDialogViewModel(string message)
    {
        this.Message = message;
    }
}