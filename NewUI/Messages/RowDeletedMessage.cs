using CommunityToolkit.Mvvm.Messaging.Messages;

namespace ROGraph.NewUI.Messages;

public class RowDeletedMessage : ValueChangedMessage<int>
{
    public RowDeletedMessage(int index) : base(index)
    {
        
    }
}