using System;
using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using ROGraph.UI.Pages;
using ROGraph.UI.Services.StateService;
using ROGraph.UI.Views;
using ROGraph.UI.Views.ReadingOrderView;

namespace ROGraph.UI.Services;

internal class PageService : IPageService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IStateService _stateService;

    public PageService(IServiceProvider serviceProvider, IStateService stateService)
    {
        _serviceProvider = serviceProvider;
        _stateService = stateService;
    }

    public Page GetPage(PageType pageType)
    {
        return pageType switch
        {
            PageType.ReadingOrderListPage => GetPage<ReadingOrderListView>(),
            PageType.ReadingOrderPage page =>  GetReadingOrderPage(page.ReadingOrderId),
            _ => throw new InvalidOperationException($"Page type {pageType} could not be loaded")
        };
    }

    private T GetPage<T>() where T : UserControl
    {
        return _serviceProvider.GetRequiredService<T>();
    }

    private ReadingOrderView GetReadingOrderPage(Guid readingOrderId)
    {
        _stateService.SelectedReadingOrderId = readingOrderId;
        return GetPage<ReadingOrderView>();
    }
}
