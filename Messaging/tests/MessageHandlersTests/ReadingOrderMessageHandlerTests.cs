using System;
using System.Collections.Generic;
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
    public async Task GetReadingOrderRequest_CallsService()
    {
        // Arrange
        var readingOrder = new ReadingOrder("Existing Reading Order", Guid.NewGuid());
        var message = new GetReadingOrderRequest(readingOrder.Id);
        _mocker.Setup<IReadingOrderService, Task<ReadingOrder?>>(s =>
                s.GetReadingOrder(readingOrder.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(readingOrder);
        
        // Act
        var result = await WeakReferenceMessenger.Default.Send(message);
        
        // Assert
        Assert.That(result, Is.EqualTo(readingOrder));
    }

    [Test]
    public async Task GetReadingOrderOverviewRequest_CallsService()
    {
        // Arrange
        var readingOrder = new ReadingOrderOverview("Existing Reading Order", Guid.NewGuid());
        var message = new GetReadingOrderOverviewRequest(readingOrder.Id);
        _mocker.Setup<IReadingOrderService, Task<ReadingOrderOverview?>>(s =>
                s.GetReadingOrderOverview(readingOrder.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(readingOrder);

        // Act
        var result = await WeakReferenceMessenger.Default.Send(message);
        
        // Assert
        Assert.That(result, Is.EqualTo(readingOrder));
    }

    public async Task GetReadingOrderOverviewsRequest_CallsService()
    {
        // Arrange
        var readingOrders = new List<ReadingOrderOverview>
        {
            new("Reading Order 1", Guid.NewGuid()),
            new("Reading Order 2", Guid.NewGuid()),
        };
        var message = new GetReadingOrderOverviewsRequest();
        _mocker.Setup<IReadingOrderService, Task<IList<ReadingOrderOverview>>>(s =>
                s.GetReadingOrderOverviews(It.IsAny<CancellationToken>()))
            .ReturnsAsync(readingOrders);
        
        // Act
        var result = await WeakReferenceMessenger.Default.Send(message);
        
        // Assert
        Assert.That(result, Is.EquivalentTo(readingOrders));
    }

    [Test]
    public async Task AddReadingOrderRequest_CallsService()
    {
        // Arrange
        var readingOrder = new ReadingOrderOverview("New Reading Order", Guid.NewGuid(), "", 2, 2);
        var message = new AddReadingOrderRequest(readingOrder);
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
    public async Task UpdateReadingOrderRequest_CallsService()
    {
        // Arrange
        var readingOrder = new ReadingOrderOverview("My New Reading Order", Guid.NewGuid());
        var message = new UpdateReadingOrderRequest(readingOrder);
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
    public async Task UpdateReadingOrderContentRequest_CallsService()
    {
        // Arrange
        var readingOrder = new ReadingOrder("My New Reading Order", Guid.NewGuid());
        var message = new UpdateReadingOrderContentRequest(readingOrder);
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
    public async Task DeleteReadingOrderRequest_CallsService()
    {
        // Arrange
        var readingOrderId = Guid.NewGuid();
        var message = new DeleteReadingOrderRequest(readingOrderId);
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
