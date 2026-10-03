using System;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace ROGraph.Messaging.Messages;

public class DeleteReadingOrderMessage : AsyncRequestMessage<bool>
{
    public Guid ReadingOrderId;

    public DeleteReadingOrderMessage(Guid readingOrderId)
    {
        ReadingOrderId = readingOrderId;
    }
}
