using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ROGraph.NewUI.Messages;
using ROGraph.NewUI.Models;
using ROGraph.Shared.Models;

namespace ROGraph.NewUI.Dispatchers;

internal partial class ReadingOrderViewDispatcher
{
    public static ICommand DispatchNodeAddedCommand { get; } = new RelayCommand<Node>(DispatchNodeAddedMessage);
    public static ICommand DispatchNodeDeletedCommand { get; } = new RelayCommand<Guid>(DispatchNodeDeletedMessage);

    private static void DispatchNodeAddedMessage(Node? node)
    {
        if(node == null)
        {
            return;
        }

        var message = new NodeAddedMessage(node);
        WeakReferenceMessenger.Default.Send(message);
    }

    private static void DispatchNodeDeletedMessage(Guid id)
    {
        var message = new NodeDeletedMessage(id);
        WeakReferenceMessenger.Default.Send(message);
    }
}