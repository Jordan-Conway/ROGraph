using ROGraph.UI.ViewModels;

namespace ROGraph.UI.Dialogs.ConfirmDialog;

public class ConfirmDialogViewModel : ViewModelBase
{
    public string Message { get; set; }

    public ConfirmDialogViewModel(string message)
    {
        this.Message = message;
    }
}