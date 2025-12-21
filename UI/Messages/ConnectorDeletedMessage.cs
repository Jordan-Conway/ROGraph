using System;
using CommunityToolkit.Mvvm.Messaging.Messages;
using ROGraph.Shared.Models;

namespace ROGraph.UI.Messages;

internal class ConnectorDeletedMessage : ValueChangedMessage<Guid>
{
    public ConnectorDeletedMessage(Guid id) : base(id)
    {
        
    }
}