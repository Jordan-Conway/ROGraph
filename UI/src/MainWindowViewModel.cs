using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using ROGraph.UI.Messages;
using System;
using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using ROGraph.UI.Controls;
using ROGraph.UI.Services;
using ROGraph.UI.Pages;

namespace ROGraph.UI;

internal partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private Page _currentPage;

    [ObservableProperty]
    private IDialogContainer _dialogContainer;

    private readonly IMessenger _messenger;
    private readonly IPageService _pageService;

    public MainWindowViewModel(IMessenger messenger, IPageService pageService, [FromKeyedServices("dialogContainer")] IDialogContainer dialogContainer )
    {
        _messenger = messenger;
        _pageService = pageService;

        CurrentPage = _pageService.GetPage(new PageType.ReadingOrderListPage());
        DialogContainer = dialogContainer;

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
