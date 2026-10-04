using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using ROGraph.Backend.Context;
using ROGraph.Backend.DatabaseModels;
using ROGraph.Backend.Repositories.ReadingOrders;
using ROGraph.Shared.Models;

namespace ROGraph.Backend.Tests.Repositories;

public sealed class ReadingOrderRepositoryTests : RepositoryTest
{
    private ReadingOrderRepository _repository;

    [SetUp]
    public void Setup()
    {
        _repository = Mocker.CreateInstance<ReadingOrderRepository>();
    }

    [Test]
    public async Task GetAllReadingOrders_ReturnsAllReadingOrdersThatAreNotDeleted()
    {
        // Act
        var result = await _repository.GetAllReadingOrders(TestContext.CurrentContext.CancellationToken);

        // Assert
        var expectedReadingOrders = new List<ReadingOrderOverview> {
            ExistingReadingOrder.ToOverview()
        };

        Assert.That(result, Is.EquivalentTo(expectedReadingOrders));
    }

    [Test]
    public async Task GetReadingOrder_ReadingOrderExists_ReturnsReadingOrder()
    {
        // Act
        var result = await _repository.GetReadingOrder(ExistingReadingOrderId, TestContext.CurrentContext.CancellationToken);

        // Assert
        Assert.That(result, Is.EqualTo(ExistingReadingOrder.ToOverview()));
    }

    [Test]
    public async Task GetReadingOrder_ReadingOrderDoesNotExist_ReturnsNull()
    {
        // Act
        var result = await _repository.GetReadingOrder(Guid.NewGuid(), TestContext.CurrentContext.CancellationToken);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task CreateReadingOrder_ReadingOrderCreated()
    {
        // Arrange
        var readingOrder = new ReadingOrderOverview("New Reading Order", Guid.NewGuid(), "New Description", 5, 4);
        Mocker.Setup<IDBContext, Task<int>>(c => c.Save(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        var result = await _repository.CreateReadingOrder(readingOrder, TestContext.CurrentContext.CancellationToken);

        // Assert
        Assert.That(result, Is.True);

        var expectedReadingOrder = readingOrder.ToDbModel(ReadingOrderStatus.Active);
        Mocker.Verify<IDBContext>(c => c.Add(expectedReadingOrder, TestContext.CurrentContext.CancellationToken), Times.Once);
    }

    [Test]
    public async Task CreateReadingOrder_EmptyId_CreatesNewId()
    {
        // Arrange
        var readingOrder = new ReadingOrderOverview("New Reading Order", Guid.Empty, "New Description", 5, 4);

        // Act
        var result = await _repository.CreateReadingOrder(readingOrder, TestContext.CurrentContext.CancellationToken);

        // Assert
        Mocker.Verify<IDBContext>(c => c.Add(It.Is<ReadingOrderOverviewDbModel>(o => o.Id != Guid.Empty), TestContext.CurrentContext.CancellationToken), Times.Once);
    }
}
