using CommunityToolkit.Mvvm.Messaging.Messages;
using ROGraph.Shared.Models;

namespace ROGraph.Messaging.Messages;

public class AddReadingOrderMessage : AsyncRequestMessage<bool>
{
    public ReadingOrderOverview Overview;

    public AddReadingOrderMessage(ReadingOrderOverview readingOrder)
    {
        Overview = readingOrder;
    }
}
