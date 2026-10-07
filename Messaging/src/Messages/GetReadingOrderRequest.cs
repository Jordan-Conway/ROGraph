using System;
using CommunityToolkit.Mvvm.Messaging.Messages;
using ROGraph.Shared.Models;

namespace ROGraph.Messaging.Messages;

public class GetReadingOrderRequest : AsyncRequestMessage<ReadingOrder?>
{
    public Guid ReadingOrderId;

    public GetReadingOrderRequest(Guid readingOrderId)
    {
        ReadingOrderId = readingOrderId;
    }
}