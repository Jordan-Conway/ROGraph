using Avalonia.Controls;
using CommunityToolkit.Mvvm.Messaging.Messages;
using ROGraph.UI.Pages;

namespace ROGraph.UI.Messages;

internal class NavigationMessage : ValueChangedMessage<PageType>
{
    public NavigationMessage(PageType pageType) : base(pageType)
    {

    }
}
