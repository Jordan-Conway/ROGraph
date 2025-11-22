using Avalonia.Controls;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace ROGraph.NewUI.Messages;

public class NavigationMessage : ValueChangedMessage<UserControl>
{
    public NavigationMessage(UserControl userControl) : base(userControl)
    {
        
    }
}