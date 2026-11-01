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

    private readonly IMessenger _messenger;
    private readonly IPageService _pageService;

    public MainWindowViewModel(IMessenger messenger, IPageService pageService)
    {
        _messenger = messenger;
        _pageService = pageService;

        CurrentPage = _pageService.GetPage(new PageType.ReadingOrderListPage());

        this.RegisterMessages();
    }

    private void RegisterMessages()
    {
        _messenger.Register<NavigationMessage>(this, (r, m) =>
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
