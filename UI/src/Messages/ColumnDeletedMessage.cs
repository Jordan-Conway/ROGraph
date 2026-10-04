using CommunityToolkit.Mvvm.Messaging.Messages;

namespace ROGraph.UI.Messages;

public class ColumnDeletedMessage : ValueChangedMessage<int>
{
    public ColumnDeletedMessage(int position) : base(position)
    {
        
    }
}