using CommunityToolkit.Mvvm.Messaging.Messages;

namespace ROGraph.UI.Messages;

public class RowAddedMessage : ValueChangedMessage<int>
{
    public RowAddedMessage(int index) : base(index)
    {
        
    }
}