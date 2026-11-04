using Avalonia;
using Avalonia.Controls;
using ROGraph.UI.Dialogs;

namespace ROGraph.UI.Controls;

internal partial class DialogContainer : UserControl, IDialogContainer
{
    public static readonly StyledProperty<BaseDialog?> DialogControlProperty = AvaloniaProperty.Register<DialogContainer, BaseDialog?>(nameof(DialogControl));
    
    public BaseDialog? DialogControl
    {
        get => GetValue(DialogControlProperty);
        set => SetValue(DialogControlProperty, value);
    }
    
    public DialogContainer()
    {
        InitializeComponent();
    }
}