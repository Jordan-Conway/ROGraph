using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Xaml.Interactions.Core;
using CommunityToolkit.Mvvm.Input;

namespace ROGraph.UI.Components.Toolbar;

internal partial class ToolbarItem
{
    public string Label { get; set; }
    public Func<Task> OnClick;

    public ToolbarItem(string label, Func<Task> onClick)
    {
        Label = label;
        OnClick = onClick;
    }
    
    [RelayCommand]
    public void InvokeAction()
    {
        OnClick?.Invoke();
    }
}