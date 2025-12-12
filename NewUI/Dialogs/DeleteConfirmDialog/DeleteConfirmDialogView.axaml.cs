using Avalonia.Controls;
using Avalonia.Interactivity;

namespace ROGraph.NewUI.Dialogs.DeleteConfirmDialog;

internal partial class DeleteConfirmDialogView : Window
{
    public DeleteConfirmDialogView(string name)
    {
        InitializeComponent();

        this.DataContext = new DeleteConfirmDialogViewModel(name);
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