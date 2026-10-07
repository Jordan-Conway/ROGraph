using CommunityToolkit.Mvvm.Messaging.Messages;
using ROGraph.Shared.Models;

namespace ROGraph.UI.Messages;

public class EditReadingOrderMessage : AsyncRequestMessage<ReadingOrderOverview?>
{
    public ReadingOrderOverview Overview { get; }

    public EditReadingOrderMessage(ReadingOrderOverview overview)
    {
        Overview = overview;
    }
}