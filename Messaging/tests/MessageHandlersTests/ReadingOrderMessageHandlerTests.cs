using System;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using Moq;
using Moq.AutoMock;
using ROGraph.Backend.Contracts;
using ROGraph.Messaging.MessageHandlers;
using ROGraph.Messaging.Messages;
using ROGraph.Shared.Models;

namespace ROGraph.Messaging.Tests.MessageHandlersTests;

public sealed class ReadingOrderMessageHandlerTests
{
    private readonly AutoMocker _mocker = new();
    private ReadingOrderMessageHandler _handler;

    [OneTimeSetUp]
    public void Setup()
    {
        _handler = _mocker.CreateInstance<ReadingOrderMessageHandler>();
    }

    [Test]
    public async Task AddReadingOrderMessage_CallsService()
    {
        // Arrange
        var readingOrder = new ReadingOrderOverview("New Reading Order", Guid.NewGuid(), "", 2, 2);
        var message = new AddReadingOrderMessage(readingOrder);
        _mocker.Setup<IReadingOrderService, Task<bool>>(s =>
                s.CreateReadingOrder(It.IsAny<ReadingOrderOverview>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        
        // Act
        var result = await WeakReferenceMessenger.Default.Send(message);
        
        // Assert
        Assert.That(result, Is.True);
        
        _mocker.Verify<IReadingOrderService>(s => s.CreateReadingOrder(readingOrder,  It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task UpdateReadingOrderMessage_CallsService()
    {
        // Arrange
        var readingOrder = new ReadingOrderOverview("My New Reading Order", Guid.NewGuid());
        var message = new UpdateReadingOrderMessage(readingOrder);
        _mocker.Setup<IReadingOrderService, Task<bool>>(s =>
                s.UpdateReadingOrderOverview(It.IsAny<ReadingOrderOverview>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        
        // Act
        var result = await WeakReferenceMessenger.Default.Send(message);

        // Assert
        Assert.That(result, Is.True);
        
        _mocker.Verify<IReadingOrderService>(s => s.UpdateReadingOrderOverview(readingOrder, It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Test]
    public async Task UpdateReadingOrderContentMessage_CallsService()
    {
        // Arrange
        var readingOrder = new ReadingOrder("My New Reading Order", Guid.NewGuid());
        var message = new UpdateReadingOrderContentMessage(readingOrder);
        _mocker.Setup<IReadingOrderService, Task<bool>>(s =>
                s.UpdateReadingOrder(It.IsAny<ReadingOrder>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        
        // Act
        var result = await WeakReferenceMessenger.Default.Send(message);

        // Assert
        Assert.That(result, Is.True);
        
        _mocker.Verify<IReadingOrderService>(s => s.UpdateReadingOrder(readingOrder, It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Test]
    public async Task DeleteReadingOrderMessage_CallsService()
    {
        // Arrange
        var readingOrderId = Guid.NewGuid();
        var message = new DeleteReadingOrderMessage(readingOrderId);
        _mocker.Setup<IReadingOrderService, Task<bool>>(s =>
                s.DeleteReadingOrder(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        
        // Act
        var result = await WeakReferenceMessenger.Default.Send(message);

        // Assert
        Assert.That(result, Is.True);
        
        _mocker.Verify<IReadingOrderService>(s => s.DeleteReadingOrder(readingOrderId, It.IsAny<CancellationToken>()), Times.Once);
    }
}
