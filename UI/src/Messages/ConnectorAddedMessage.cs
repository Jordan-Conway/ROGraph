using CommunityToolkit.Mvvm.Messaging.Messages;
using ROGraph.Shared.Models;

namespace ROGraph.UI.Messages;

internal class ConnectorAddedMessage : ValueChangedMessage<Connector>
{
    public ConnectorAddedMessage(Connector connector) : base(connector)
    {
        
    }
}