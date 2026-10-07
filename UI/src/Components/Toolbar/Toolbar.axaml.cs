using Avalonia;
using Avalonia.Controls;

namespace ROGraph.UI.Components.Toolbar;

internal partial class Toolbar : UserControl
{
    public static readonly StyledProperty<ToolbarContent> ContentProperty =
        AvaloniaProperty.Register<Toolbar, ToolbarContent>(nameof(Content));

    public ToolbarContent Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }
    
    public Toolbar()
    {
        InitializeComponent();
    }
}