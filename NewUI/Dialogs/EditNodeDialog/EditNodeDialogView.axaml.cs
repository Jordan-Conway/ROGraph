using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.Messaging;
using ROGraph.NewUI.Dialogs.ConfirmDialog;
using ROGraph.NewUI.Dispatchers;
using ROGraph.NewUI.Models;
using ROGraph.Shared.Models;

namespace ROGraph.NewUI.Dialogs.EditNodeDialog;

internal partial class EditNodeDialogView : Window
{
    private bool _confirmCancel { get; set; }

    public EditNodeDialogView(Node node, bool confirmCancel = false)
    {
        InitializeComponent();

        this._confirmCancel = confirmCancel;

        this.DataContext = new EditNodeDialogViewModel(node);
    }

    private void Confirm(object? sender, RoutedEventArgs e)
    {
        WeakReferenceMessenger.Default.Send(new EditNodeDialogClosedMessage(false));

        this.Close(true);
    }

    private async void Cancel(object? sender, RoutedEventArgs e)
    {
        bool shouldClose = true;

        if(_confirmCancel)
        {
            var dialog = new ConfirmDialogView($"Are you sure you want to cancel?");
            var root = this.VisualRoot as Window;
            shouldClose = await dialog.ShowDialog<bool>(root!);
        }
        
        if(!shouldClose)
        {
            return;
        }

        WeakReferenceMessenger.Default.Send(new EditNodeDialogClosedMessage(true));

        this.Close(false);
    }
}