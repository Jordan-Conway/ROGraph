using CommunityToolkit.Mvvm.Messaging.Messages;
using ROGraph.Shared.Models;

namespace ROGraph.Messaging.Messages;

public class AddReadingOrderRequest : AsyncRequestMessage<bool>
{
    public ReadingOrderOverview Overview;

    public AddReadingOrderRequest(ReadingOrderOverview readingOrder)
    {
        Overview = readingOrder;
    }
}
