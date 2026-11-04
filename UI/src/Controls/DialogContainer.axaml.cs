using Avalonia;
using Avalonia.Controls;
using ROGraph.UI.Dialogs;

namespace ROGraph.UI.Controls;

internal partial class DialogContainer : UserControl, IDialogContainer
{
    public static readonly StyledProperty<BaseDialog?> DialogControlProperty =
        AvaloniaProperty.Register<DialogContainer, BaseDialog?>(nameof(DialogControl));

    public static readonly DirectProperty<DialogContainer, bool> HasDialogProperty =
        AvaloniaProperty.RegisterDirect<DialogContainer, bool>(nameof(HasDialog), o => o.HasDialog);
    
    public BaseDialog? DialogControl
    {
        get => GetValue(DialogControlProperty);
        set => SetValue(DialogControlProperty, value);
    }

    public bool HasDialog
    {
        get;
        private set => SetAndRaise(HasDialogProperty, ref field, value);
    }

    public DialogContainer()
    {
        InitializeComponent();
    }

    static DialogContainer()
    {
        DialogControlProperty.Changed.AddClassHandler<DialogContainer>((container, _) =>
        {
            container.HasDialog = container.DialogControl != null;
        });
    }
}