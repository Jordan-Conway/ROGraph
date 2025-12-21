using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ROGraph.UI.Messages;
using ROGraph.UI.Models;
using ROGraph.Shared.Models;

namespace ROGraph.UI.Dispatchers;

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

    public static void DispatchConnectorAddedMessage(Connector connector)
    {
        var message = new ConnectorAddedMessage(connector);
        WeakReferenceMessenger.Default.Send(message);
    }

    public static void DispatchConnectorDeletedMessage(Guid id)
    {
        var message = new ConnectorDeletedMessage(id);
        WeakReferenceMessenger.Default.Send(message);
    }

    public static void DispatchColumnAddedEvent(int index)
    {
        var message = new ColumnAddedMessage(index);
        WeakReferenceMessenger.Default.Send(message); 
    }

    public static void DispatchColumnDeletedEvent(int index)
    {
        var message = new ColumnDeletedMessage(index);
        WeakReferenceMessenger.Default.Send(message); 
    }

    public static void DispatchRowAddedEvent(int index)
    {
        var message = new RowAddedMessage(index);
        WeakReferenceMessenger.Default.Send(message);
    }

    public static void DispatchRowDeletedEvent(int index)
    {
        var message = new RowDeletedMessage(index);
        WeakReferenceMessenger.Default.Send(message); 
    }
}