using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using ROGraph.Backend.Context;
using ROGraph.Backend.DatabaseModels;
using ROGraph.Backend.Repositories.Nodes;
using ROGraph.Shared.Enums;
using ROGraph.Shared.Models;

namespace ROGraph.Backend.Tests.Repositories;

public sealed class NodeRepositoryTests : RepositoryTest
{
    private NodeRepository _repository;

    [SetUp]
    public void Setup()
    {
        _repository = Mocker.CreateInstance<NodeRepository>();
    }

    [Test]
    public async Task GetNodesForReadingOrder_ReturnsNodesPlacedInReadingOrder()
    {
        // Act
        var result =
            await _repository.GetNodesForReadingOrder(ExistingReadingOrderId, TestContext.CurrentContext.CancellationToken);
        
        // Assert
        var context = Mocker.GetMock<IDBContext>().Object;
        var expectedNodes = context.GetSet<NodeDbModel>().Take(2).Select(n => n.ToNode());
        Assert.That(result, Is.EquivalentTo(expectedNodes));
    }

    [Test]
    public async Task GetNodesForReadingOrder_NoNodesForReadingOrder_ReturnsEmptyList()
    {
        // Act
        var result = await _repository.GetNodesForReadingOrder(Guid.NewGuid(), TestContext.CurrentContext.CancellationToken);
        
        // Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task CreateNode_NodeDoesNotExist_NodeCreated()
    {
        // Arrange
        var nodeToCreate = new Node
        {
            Id = Guid.NewGuid(),
            Name = "New Node",
            Origin = ExistingReadingOrderId
        };
        var placementToCreate = (4, 1);
        
        // Act
        var result = await _repository.CreateNode(nodeToCreate, ExistingReadingOrderId, placementToCreate,
            TestContext.CurrentContext.CancellationToken);
        
        // Assert
        Assert.That(result, Is.EqualTo(nodeToCreate.Id));
        
        var expectedNode = nodeToCreate.ToDbModel();
        var expectedPlacement = new NodePlacementDbModel
        {
            NodeId = nodeToCreate.Id,
            ReadingOrderId = ExistingReadingOrderId,
            X = placementToCreate.Item1,
            Y = placementToCreate.Item2
        };
        Mocker.Verify<IDBContext>(c => c.Add(expectedNode, TestContext.CurrentContext.CancellationToken), Times.Once);
        Mocker.Verify<IDBContext>(c => c.Add(expectedPlacement, TestContext.CurrentContext.CancellationToken), Times.Once);

    }

    [Test]
    public async Task CreateNode_NodeAlreadyExists_SkipsNodeAndCreatesPlacement()
    {
        // Arrange
        var nodeToCreate = new Node() with
        {
            Id = ExistingNodeId1
        };
        var placementToCreate = (3, 4);
        
        // Act
        var result = await _repository.CreateNode(nodeToCreate, ExistingReadingOrderId, placementToCreate,
            TestContext.CurrentContext.CancellationToken);
        
        // Asset
        Assert.That(result, Is.EqualTo(ExistingNodeId1));

        var expectedPlacement = new NodePlacementDbModel()
        {
            NodeId = ExistingNodeId1,
            ReadingOrderId = ExistingReadingOrderId,
            X = placementToCreate.Item1,
            Y = placementToCreate.Item2
        };
        Mocker.Verify<IDBContext>(c => c.Add(expectedPlacement, TestContext.CurrentContext.CancellationToken), Times.Once);
        Mocker.Verify<IDBContext>(c => c.Add(It.IsAny<NodeDbModel>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test]
    public async Task UpdateNode_UpdatesNodeAndPlacement()
    {
        // Arrange
        var updatedNode = new Node(
            ExistingNodeId1,
            "New Name",
            ExistingReadingOrderId,
            DateTime.Now,
            DateTime.Now,
            Guid.NewGuid(),
            Guid.NewGuid(),
            NodeType.TRIANGLE,
            true,
            null,
            "A new Descriptions"
        );
        var updatedPlacement = (5, 6);

        Mocker.Setup<IDBContext, Task<int>>(c => c.Save(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        var result = await _repository.UpdateNode(updatedNode, updatedPlacement, ExistingReadingOrderId,
            TestContext.CurrentContext.CancellationToken);
        
        // Assert
        Assert.That(result, Is.True);

        var context = Mocker.GetMock<IDBContext>().Object;
        var updatedNodeInDb = context.GetSet<NodeDbModel>().First(n => n.Id == ExistingNodeId1);
        var updatedPlacementInDb = context.GetSet<NodePlacementDbModel>().First(p => p.NodeId == ExistingNodeId1);
        
        AssertNodeIsAsExpected(updatedNode, updatedNodeInDb);
        Assert.Multiple(() =>
        {
            Assert.That(updatedPlacementInDb.ReadingOrderId, Is.EqualTo(ExistingReadingOrderId));
            Assert.That(updatedPlacementInDb.NodeId, Is.EqualTo(ExistingNodeId1));
            Assert.That(updatedPlacementInDb.X, Is.EqualTo(updatedPlacement.Item1));
            Assert.That(updatedPlacementInDb.Y, Is.EqualTo(updatedPlacement.Item2));
        });
    }

    [Test]
    public async Task UpdateNode_PlacementDoesNotExist_CreatesPlacement()
    {
        // Arrange
        var newReadingOrderId = Guid.NewGuid();
        var updatedNode = new Node(
            ExistingNodeId1,
            "New Name",
            ExistingReadingOrderId,
            DateTime.Now,
            DateTime.Now,
            Guid.NewGuid(),
            Guid.NewGuid(),
            NodeType.TRIANGLE,
            true,
            null,
            "A new Descriptions"
        );
        var newPlacement = (5, 6);

        Mocker.Setup<IDBContext, Task<int>>(c => c.Save(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        var result = await _repository.UpdateNode(updatedNode, newPlacement, newReadingOrderId,
            TestContext.CurrentContext.CancellationToken);
        
        // Assert
        Assert.That(result, Is.True);
        
        var context = Mocker.GetMock<IDBContext>().Object;
        var updatedNodeInDb = context.GetSet<NodeDbModel>().First(n => n.Id == ExistingNodeId1);
        
        AssertNodeIsAsExpected(updatedNode, updatedNodeInDb);

        var expectedPlacement = new NodePlacementDbModel
        {
            NodeId = updatedNode.Id,
            ReadingOrderId = newReadingOrderId,
            X = newPlacement.Item1,
            Y = newPlacement.Item2
        };
        Mocker.Verify<IDBContext>(c => c.Add(expectedPlacement, TestContext.CurrentContext.CancellationToken), Times.Once);
    }

    private static void AssertNodeIsAsExpected(Node updatedNode, NodeDbModel nodeInDatabase)
    {
        Assert.Multiple(() =>
        {
            Assert.That(nodeInDatabase.Name, Is.EqualTo(updatedNode.Name));
            Assert.That(nodeInDatabase.Origin, Is.EqualTo(updatedNode.Origin));
            Assert.That(nodeInDatabase.Created, Is.EqualTo(FakeCreationDate));
            Assert.That(nodeInDatabase.Type, Is.EqualTo(updatedNode.Type));
            Assert.That(nodeInDatabase.IsCompleted, Is.EqualTo(updatedNode.IsCompleted));
            Assert.That(nodeInDatabase.ChecklistId, Is.EqualTo(updatedNode.Checklist?.Id ?? Guid.Empty));
            Assert.That(nodeInDatabase.Description, Is.EqualTo(updatedNode.Description));
        });

    }
}