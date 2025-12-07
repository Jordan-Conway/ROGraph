using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.Messaging;
using ROGraph.NewUI.Dispatchers;
using ROGraph.NewUI.Models;
using ROGraph.Shared.Models;

namespace ROGraph.NewUI.Dialogs.EditNodeDialog;

internal partial class EditNodeDialogView : Window
{
    public EditNodeDialogView(NodeModel nodeModel)
    {
        InitializeComponent();

        this.DataContext = new EditNodeDialogViewModel(nodeModel);
    }

    private void Confirm(object? sender, RoutedEventArgs e)
    {
        WeakReferenceMessenger.Default.Send(new EditNodeDialogClosedMessage(false));

        this.Close();
    }

    private void Cancel(object? sender, RoutedEventArgs e)
    {
        WeakReferenceMessenger.Default.Send(new EditNodeDialogClosedMessage(true));

        this.Close();
    }
}