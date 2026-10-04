using System;
using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace ROGraph.UI.Services;

internal class PageService : IPageService
{
    private readonly IServiceProvider _serviceProvider;

    public PageService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public T GetPage<T>() where T : UserControl
    {
        return _serviceProvider.GetRequiredService<T>();
    }
}