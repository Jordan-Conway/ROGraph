using Avalonia.Controls;
using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.Messaging;
using ROGraph.UI.Dialogs.ConfirmDialog;
using ROGraph.Shared.Models;

namespace ROGraph.UI.Dialogs.EditNodeDialog;

internal partial class EditNodeDialogView : Window
{
    private bool ConfirmCancel { get; set; }

    public EditNodeDialogView(Node node, bool confirmCancel = false)
    {
        InitializeComponent();

        ConfirmCancel = confirmCancel;

        DataContext = new EditNodeDialogViewModel(node);
    }

    private void Confirm(object? sender, RoutedEventArgs e)
    {
        WeakReferenceMessenger.Default.Send(new EditNodeDialogClosedMessage(false));

        Close(true);
    }

    private async void Cancel(object? sender, RoutedEventArgs e)
    {
        var shouldClose = true;

        if(ConfirmCancel)
        {
            var dialog = new ConfirmDialogView($"Are you sure you want to cancel?");
            var root = VisualRoot as Window;
            shouldClose = await dialog.ShowDialog<bool>(root!);
        }
        
        if(!shouldClose)
        {
            return;
        }

        WeakReferenceMessenger.Default.Send(new EditNodeDialogClosedMessage(true));

        Close(false);
    }
}