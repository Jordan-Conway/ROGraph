using System;
using CommunityToolkit.Mvvm.Messaging.Messages;
using ROGraph.Shared.Models;

namespace ROGraph.Messaging.Messages;

public class GetReadingOrderOverviewRequest : AsyncRequestMessage<ReadingOrderOverview?>
{
    public Guid ReadingOrderId;

    public GetReadingOrderOverviewRequest(Guid readingOrderId)
    {
        ReadingOrderId = readingOrderId;
    }
}