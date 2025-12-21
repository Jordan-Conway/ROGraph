using CommunityToolkit.Mvvm.Messaging.Messages;
using ROGraph.UI.Models;

namespace ROGraph.UI.Messages;

internal class NodeAddedMessage : ValueChangedMessage<NodeModel>
{
    public NodeAddedMessage(NodeModel nodeModel) : base(nodeModel)
    {

    }
}