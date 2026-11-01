using Avalonia.Controls;
using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.Messaging;
using ROGraph.UI.Dialogs.ConfirmDialog;
using ROGraph.Shared.Models;

namespace ROGraph.UI.Dialogs.EditNodeDialog;

internal partial class EditNodeDialogView : Window
{
    private readonly IMessenger _messenger;
    private bool ConfirmCancel { get; set; }

    public EditNodeDialogView(Node node, IMessenger messenger, bool confirmCancel = false)
    {
        _messenger = messenger;
        
        InitializeComponent();

        ConfirmCancel = confirmCancel;

        DataContext = new EditNodeDialogViewModel(node, messenger);
    }

    private void Confirm(object? sender, RoutedEventArgs e)
    {
        _messenger.Send(new EditNodeDialogClosedMessage(false));

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

        _messenger.Send(new EditNodeDialogClosedMessage(true));

        Close(false);
    }
}