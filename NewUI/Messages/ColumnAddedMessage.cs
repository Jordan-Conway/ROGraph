using System;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace ROGraph.NewUI.Messages;

public class ColumnAddedMessage : ValueChangedMessage<int>
{
    public ColumnAddedMessage(int position) : base(position)
    {
        
    }
}