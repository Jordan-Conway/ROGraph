using CommunityToolkit.Mvvm.Messaging.Messages;
using ROGraph.Shared.Models;

namespace ROGraph.Messaging.Messages;

// TODO: Use a union and merge this into UpdateReadingOrderMessage when C#15 is released
public class UpdateReadingOrderContentRequest : AsyncRequestMessage<bool>
{
    public ReadingOrder ReadingOrder;

    public UpdateReadingOrderContentRequest(ReadingOrder readingOrder)
    {
        ReadingOrder = readingOrder;
    }
}
