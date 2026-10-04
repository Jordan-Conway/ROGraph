using CommunityToolkit.Mvvm.Messaging.Messages;
using ROGraph.Shared.Models;

namespace ROGraph.UI.Messages;

public class SaveReadingOrderMessage : AsyncRequestMessage<bool>
{  
    public ReadingOrder ReadingOrder { get; }
    public SaveReadingOrderMessage(ReadingOrder readingOrder)
    {
        ReadingOrder = readingOrder;
    }
}