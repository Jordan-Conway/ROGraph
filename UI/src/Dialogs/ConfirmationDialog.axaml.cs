using Avalonia.Controls;

namespace ROGraph.UI.Dialogs;

internal partial class ConfirmationDialog : Dialog<bool>
{
    public string Message { get; set; }

    public ConfirmationDialog(string message)
    {
        Message = message;
        InitializeComponent();
    }

    public void Confirm()
    {
        Close(true);
    }

    public void Cancel()
    {
        Close(false);
    }
}