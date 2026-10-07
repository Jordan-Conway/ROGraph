using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using ROGraph.UI.Messages;
using System;
using ROGraph.UI.Services;
using ROGraph.UI.Pages;

namespace ROGraph.UI;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private Page _currentPage;
    private readonly IMessagingService _messagingService;
    private readonly IPageService _pageService;

    public MainWindowViewModel(IMessagingService messagingService, IPageService pageService)
    {
        _messagingService = messagingService;
        _pageService = pageService;

        CurrentPage = _pageService.GetPage(new PageType.ReadingOrderListPage());

        this.RegisterMessages();
    }

    private void RegisterMessages()
    {
        WeakReferenceMessenger.Default.Register<NavigationMessage>(this, (r, m) =>
        {
            this.ChangeView(m.Value);
        });
    }

    private void ChangeView(PageType pageType)
    {
        Console.WriteLine("Moving to new view");
        var page = _pageService.GetPage(pageType);
        CurrentPage = page;
    }
}
