using CommunityToolkit.Mvvm.Messaging.Messages;

namespace ROGraph.NewUI.Messages;

public class RowAddedMessage : ValueChangedMessage<int>
{
    public RowAddedMessage(int index) : base(index)
    {
        
    }
}