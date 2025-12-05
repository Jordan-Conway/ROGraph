using System;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace ROGraph.NewUI.Models;

internal class NodeDeletedMessage : ValueChangedMessage<Guid>
{
    public NodeDeletedMessage(Guid id) : base(id)
    {
        
    }
}