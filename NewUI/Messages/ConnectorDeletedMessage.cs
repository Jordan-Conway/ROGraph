using CommunityToolkit.Mvvm.Messaging.Messages;
using ROGraph.Shared.Models;

namespace ROGraph.NewUI.Messages;

internal class ConnectorDeletedMessage : ValueChangedMessage<Connector>
{
    public ConnectorDeletedMessage(Connector connector) : base(connector)
    {
        
    }
}