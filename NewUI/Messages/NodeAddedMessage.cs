using CommunityToolkit.Mvvm.Messaging.Messages;
using ROGraph.Shared.Models;

namespace ROGraph.NewUI.Messages;

internal class NodeAddedMessage : ValueChangedMessage<Node>
{
    public NodeAddedMessage(Node node) : base(node)
    {

    }
}