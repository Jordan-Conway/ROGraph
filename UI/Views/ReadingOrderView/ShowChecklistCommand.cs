using System;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace ROGraph.UI.Views.ReadingOrderView;

internal class ShowChecklistCommand : AsyncRequestMessage<bool>
{
    public Guid ReadingOrderId { get; init; }
    
    public ShowChecklistCommand(Guid readingOrderId)
    {
        ReadingOrderId = readingOrderId;
    }
}