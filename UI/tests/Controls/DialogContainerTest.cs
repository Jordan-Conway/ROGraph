using ROGraph.UI.Controls;
using ROGraph.UI.Tests.Utils;

namespace ROGraph.UI.Tests.Controls;

[TestFixture]
public class DialogContainerTest
{
    [Test]
    public void HasDialog_IsTrueWhenDialogPresent()
    {
        // Arrange
        var dialog = new TestDialog();
        var container = new DialogContainer
        {
            DialogControl = dialog
        };

        // Act
        var hasDialog = container.HasDialog;
        
        // Assert
        Assert.That(hasDialog, Is.True);
    }
    
    [Test]
    public void HasDialog_IsFalseWhenNoDialogPresent()
    {
        // Arrange
        var container = new DialogContainer();

        // Act
        var hasDialog = container.HasDialog;
        
        // Assert
        Assert.That(hasDialog, Is.False);
    }
    
    [TestCase(true, false),
    TestCase(false, true)]
    public void HasDialog_ChangesWhenDialogAddedAndRemoved(bool start, bool end)
    {
        // Arrange
        var dialog = new TestDialog();
        var container = start ? new DialogContainer
        {
            DialogControl = dialog
        } : new DialogContainer();

        // Act
        container.DialogControl = end ? dialog : null;
        var hasDialog = container.HasDialog;
        
        // Assert
        Assert.That(hasDialog, Is.EqualTo(end));
    }
}