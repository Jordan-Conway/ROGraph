using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Moq.AutoMock;
using Moq.EntityFrameworkCore;
using ROGraph.Backend.Context;
using ROGraph.Backend.DatabaseModels;
using ROGraph.Shared.Enums;

namespace ROGraph.Backend.Tests.Repositories;

public abstract class RepositoryTest
{
    protected static readonly Guid ExistingReadingOrderId = Guid.NewGuid();
    internal static readonly ReadingOrderOverviewDbModel ExistingReadingOrder = new("Existing Reader Order", ExistingReadingOrderId, "A description", 2, 2);

    private static readonly ReadingOrderOverviewDbModel DeletedReadingOrder =
        new ReadingOrderOverviewDbModel("Deleted Reading Order", Guid.NewGuid()) with
        {
            Status = ReadingOrderStatus.Deleted
        };
    protected static readonly Guid ExistingNodeId1 = Guid.NewGuid();
    protected static readonly Guid ExistingNodeId2 = Guid.NewGuid();
    protected static readonly Guid ExistingConnectorId1 = Guid.NewGuid();
    protected static readonly Guid ExistingConnectorId2 = Guid.NewGuid();
    protected static readonly DateTime FakeCreationDate = DateTime.MinValue;

    protected readonly AutoMocker Mocker = new();

    [SetUp]
    public void BaseSetup()
    {
        Mocker.Setup<IDBContextFactory, IDBContext>(f => f.CreateDbContext())
            .Returns(() => Mocker.GetMock<IDBContext>().Object);

        SetupDatabase();
    }

    private void SetupDatabase()
    {
        var nodes = new List<NodeDbModel>
        {
            CreateNodeModel("Node A", ExistingReadingOrderId) with
            {
                Id = ExistingNodeId1
            },
            CreateNodeModel("Node B", ExistingReadingOrderId) with
            {
                Id = ExistingNodeId2
            },
            CreateNodeModel("Node C", ExistingReadingOrderId),
        };
        var placements = new List<NodePlacementDbModel>
        {
            CreateNodePlacement(ExistingReadingOrderId, nodes[0].Id, 0, 0),
            CreateNodePlacement(ExistingReadingOrderId, nodes[1].Id, 1, 0),
            CreateNodePlacement(Guid.NewGuid(), nodes[2].Id, 0, 0)
        };
        var connectors = new List<ConnectorDbModel>
        {
            new()
            {
                Id = ExistingConnectorId1,
                ReadingOrderId = ExistingReadingOrderId,
                X1 = 0,
                Y1 = 0,
                X2 = 1,
                Y2 = 0,
            },
            new()
            {
                Id = ExistingConnectorId2,
                ReadingOrderId = ExistingReadingOrderId,
                X1 = 1,
                Y1 = 0,
                X2 = 2,
                Y2 = 2,
            }
        };
        var readingOrders = new List<ReadingOrderOverviewDbModel>
        {
            ExistingReadingOrder,
            DeletedReadingOrder
        };

        Mocker.Setup<IDBContext, DbSet<NodeDbModel>>(c => c.GetSet<NodeDbModel>())
            .ReturnsDbSet(nodes);
        Mocker.Setup<IDBContext, DbSet<NodePlacementDbModel>>(c => c.GetSet<NodePlacementDbModel>())
            .ReturnsDbSet(placements);
        Mocker.Setup<IDBContext, DbSet<ConnectorDbModel>>(c => c.GetSet<ConnectorDbModel>())
            .ReturnsDbSet(connectors);
        Mocker.Setup<IDBContext, DbSet<ReadingOrderOverviewDbModel>>(r => r.GetSet<ReadingOrderOverviewDbModel>())
            .ReturnsDbSet(readingOrders);
    }

    private static NodeDbModel CreateNodeModel(string name, Guid readingOrderId)
    {
        return new NodeDbModel
        {
            Id = Guid.NewGuid(),
            Name = name,
            ChecklistId = Guid.Empty,
            Description = string.Empty,
            IsCompleted = false,
            Created = FakeCreationDate,
            LastModified = FakeCreationDate,
            Origin = readingOrderId,
            Type = NodeType.CIRCLE
        };
    }

    private static NodePlacementDbModel CreateNodePlacement(Guid readingOrderId, Guid nodeId, int x, int y)
    {
        return new NodePlacementDbModel
        {
            ReadingOrderId = readingOrderId,
            NodeId = nodeId,
            X = x,
            Y = y,
        };
    }
}
