using Avalonia.Controls;
using Avalonia.Interactivity;

namespace ROGraph.NewUI.Dialogs.ConfirmDialog;

internal partial class ConfirmDialogView : Window
{
    public ConfirmDialogView(string message)
    {
        InitializeComponent();

        this.DataContext = new ConfirmDialogViewModel(message);
    }

    public void Cancel(object? sender, RoutedEventArgs e)
    {
        Close(false);
    }

    public void Confirm(object? sender, RoutedEventArgs e)
    {
        Close(true);
    }
}