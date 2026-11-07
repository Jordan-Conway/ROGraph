using ROGraph.UI.Dialogs;

namespace ROGraph.UI.Tests.Utils;

internal class TestDialog : Dialog<bool>
{
    public static bool Result { get; } = true;
}