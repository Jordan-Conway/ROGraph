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
    public static void DispatchNodeAddedMessage(NodeModel? node)
    {
        if(node == null)
        {
            return;
        }

        var message = new NodeAddedMessage(node);
        WeakReferenceMessenger.Default.Send(message);
    }

    public static void DispatchNodeEditedMessage(Node? node)
    {
        if(node == null)
        {
            return;
        }

        var message = new NodeEditedMessage(node);
        WeakReferenceMessenger.Default.Send(message);
    }

    public static void DispatchNodeDeletedMessage(Guid id)
    {
        var message = new NodeDeletedMessage(id);
        WeakReferenceMessenger.Default.Send(message);
    }
}