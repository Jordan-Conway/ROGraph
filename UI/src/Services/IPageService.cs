using System;
using Avalonia.Controls;

namespace ROGraph.UI.Services;

internal interface IPageService
{
    public T GetPage<T>() where T : UserControl;
}