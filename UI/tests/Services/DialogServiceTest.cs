using Moq;
using Moq.AutoMock;
using ROGraph.UI.Controls;
using ROGraph.UI.Dialogs.TestDialog;
using ROGraph.UI.Services;

namespace ROGraph.UI.Tests.Services;

[TestFixture]
public class DialogServiceTest
{
    private readonly AutoMocker _mocker = new AutoMocker();
    private readonly Mock<IDialogContainer> _dialogContainer;
    private readonly DialogService _service;

    public DialogServiceTest()
    {
        _dialogContainer = _mocker.GetMock<IDialogContainer>();
        _service = new DialogService(_dialogContainer.Object);

        _dialogContainer.SetupAllProperties();
    }

    [Test]
    public async Task Open_AddsAndRemovesDialogFromContainer()
    {
        // Arrange
        var dialog = new TestDialog();
        
        // Act
        var resultTask = _service.Open(dialog);
        dialog.Close(true);
        var result = await resultTask;
        
        // Assert
        Assert.That(result, Is.True);
        _dialogContainer.VerifySet(c => c.DialogControl = dialog);
        _dialogContainer.VerifySet(c => c.DialogControl = null);
    }

    [Test]
    public async Task Open_DialogAlreadyOpen_ThrowsException()
    {
        // Arrange
        var dialog1 = new TestDialog();
        var dialog2 = new TestDialog();
        
        var firstDialogTask = _service.Open(dialog1);
        
        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(() => _service.Open(dialog2));
        
        // Cleanup
        dialog1.Close(true);
        await firstDialogTask;
    }
}