using CommunityToolkit.Mvvm.Messaging.Messages;

namespace ROGraph.NewUI.Messages;

public class ColumnDeletedMessage : ValueChangedMessage<int>
{
    public ColumnDeletedMessage(int position) : base(position)
    {
        
    }
}