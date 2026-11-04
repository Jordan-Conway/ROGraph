using ROGraph.UI.Dialogs;
using ROGraph.UI.Dialogs.TestDialog;

namespace ROGraph.UI.Tests.Dialogs;

[TestFixture]
public class DialogTests
{
    [Test]
    public void Close_InvokesEventHandler()
    {
        // Arrange
        var dialog = new TestDialog();
        var closedInvoked = false;

        void Handler(object? sender, DialogClosedEventArgs<bool> args)
        {
            closedInvoked = args.Result;
        }
        dialog.CloseDialog += Handler;

        // Act
        dialog.Close(true);
        
        // Assert
        Assert.That(closedInvoked, Is.True);
    }
}