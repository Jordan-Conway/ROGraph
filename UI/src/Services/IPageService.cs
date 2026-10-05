using System;
using Avalonia.Controls;
using ROGraph.UI.Pages;

namespace ROGraph.UI.Services;

public interface IPageService
{
    public Page GetPage(PageType pageType);
}
