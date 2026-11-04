namespace ROGraph.UI.Dialogs.TestDialog;

internal partial class TestDialog : Dialog<bool>
{
    public static bool Result { get; } = true;
    
    public TestDialog()
    {
        InitializeComponent();
    }
}