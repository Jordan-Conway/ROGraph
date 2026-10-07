using CommunityToolkit.Mvvm.Messaging.Messages;
using ROGraph.Shared.Models;

namespace ROGraph.Messaging.Messages;

public class UpdateReadingOrderRequest : AsyncRequestMessage<bool>
{
    public ReadingOrderOverview Overview;

    public UpdateReadingOrderRequest(ReadingOrderOverview readingOrder)
    {
        Overview = readingOrder;
    }
}
