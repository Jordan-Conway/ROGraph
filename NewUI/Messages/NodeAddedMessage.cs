using CommunityToolkit.Mvvm.Messaging.Messages;
using ROGraph.NewUI.Models;

namespace ROGraph.NewUI.Messages;

internal class NodeAddedMessage : ValueChangedMessage<NodeModel>
{
    public NodeAddedMessage(NodeModel nodeModel) : base(nodeModel)
    {

    }
}