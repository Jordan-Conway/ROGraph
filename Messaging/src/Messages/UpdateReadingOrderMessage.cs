using CommunityToolkit.Mvvm.Messaging.Messages;
using ROGraph.Shared.Models;

namespace ROGraph.Messaging.Messages;

public class UpdateReadingOrderMessage : AsyncRequestMessage<bool>
{
    public ReadingOrderOverview Overview;

    public UpdateReadingOrderMessage(ReadingOrderOverview readingOrder)
    {
        Overview = readingOrder;
    }
}
