using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ROGraph.Backend.Contracts;
using ROGraph.Shared.Enums;
using ROGraph.Shared.Models;

namespace ROGraph.Backend.Services;

public class MockReadingOrderService : IReadingOrderService
{
    public Task<IList<ReadingOrderOverview>> GetReadingOrderOverviews(CancellationToken token = default)
    {
        IList<ReadingOrderOverview> list = new List<ReadingOrderOverview>
        {
            new("Reading Order 1", Guid.NewGuid()),
            new("Reading Order 2", Guid.NewGuid()),
        };

        return Task.FromResult(list);
    }

    public Task<ReadingOrderOverview?> GetReadingOrderOverview(Guid id, CancellationToken token = default)
    {
        ReadingOrderOverview? overview = new("Reading Order 1", Guid.NewGuid());
        return Task.FromResult(overview);
    }
    
    public Task<bool> UpdateReadingOrderOverview(ReadingOrderOverview readingOrderOverview, CancellationToken token = default)
    {
        return Task.FromResult(true);
    }

    public Task<ReadingOrder?> GetReadingOrder(Guid id, CancellationToken token = default)
    {
        var content = new ReadingOrderContentsManager();
        var coordinateTranslator = new CoordinateTranslator(2, 2);

        var x1 = coordinateTranslator.GetXFromInt(0);
        var x2 = coordinateTranslator.GetXFromInt(1);
        var y1 = coordinateTranslator.GetYFromInt(0);
        var y2 = coordinateTranslator.GetYFromInt(1);


        var node1 = new Node(Guid.NewGuid(), "Node 1", Guid.NewGuid(), DateTime.Now, DateTime.Now, x1, y1, NodeType.DIAMOND);
        var node2 = new Node(Guid.NewGuid(), "Node 2", Guid.NewGuid(), DateTime.Now, DateTime.Now, x1, y2, NodeType.DIAMOND);
        var node3 = new Node(Guid.NewGuid(), "Node 3", Guid.NewGuid(), DateTime.Now, DateTime.Now, x2, y1, NodeType.DIAMOND);

        content.AddNode(node1);
        content.AddNode(node2);
        content.AddNode(node3);

        var c1 = new Connector((x1, y1), (x2, y1));
        var c2 = new Connector((x1, y2), (x2, y1));

        content.AddConnector(c1);
        content.AddConnector(c2);

        var mockReadingOrder = new ReadingOrder("Mock Reading Order", Guid.NewGuid(), content, "This is a mock reading order")
        {
            CoordinateTranslator = coordinateTranslator
        };

        return Task.FromResult(mockReadingOrder);
    }
    
    public Task<bool> CreateReadingOrder(ReadingOrderOverview overview, CancellationToken token = default)
    {
        return Task.FromResult(true);
    }

    public Task<bool> UpdateReadingOrder(ReadingOrder readingOrder, CancellationToken token = default)
    {
        return Task.FromResult(true);
    }

    public Task<bool> DeleteReadingOrder(Guid id, CancellationToken token = default)
    {
        return Task.FromResult(true);
    }
}
