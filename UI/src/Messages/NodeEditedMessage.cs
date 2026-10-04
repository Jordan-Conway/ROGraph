using CommunityToolkit.Mvvm.Messaging.Messages;
using ROGraph.Shared.Models;

namespace ROGraph.UI.Messages;

internal class NodeEditedMessage : ValueChangedMessage<Node>
{
    public NodeEditedMessage(Node node) : base(node)
    {
        
    }
}