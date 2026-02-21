using CommunityToolkit.Mvvm.Messaging.Messages;
using ROGraph.Shared.Models;

namespace ROGraph.UI.Messages;

public class SaveReadingOrderMessage : ValueChangedMessage<ReadingOrder>
{
    public SaveReadingOrderMessage(ReadingOrder readingOrder) : base(readingOrder)
    {
        
    }
}