using System;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace ROGraph.Messaging.Messages;

public class DeleteReadingOrderRequest : AsyncRequestMessage<bool>
{
    public Guid ReadingOrderId;

    public DeleteReadingOrderRequest(Guid readingOrderId)
    {
        ReadingOrderId = readingOrderId;
    }
}
