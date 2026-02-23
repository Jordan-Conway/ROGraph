using CommunityToolkit.Mvvm.Messaging.Messages;
using ROGraph.Shared.Models;

namespace ROGraph.UI.Messages;

public class ReadingOrderAddedMessage : ValueChangedMessage<ReadingOrderOverview>
{
    public ReadingOrderAddedMessage(ReadingOrderOverview overview) : base(overview)
    {}
}