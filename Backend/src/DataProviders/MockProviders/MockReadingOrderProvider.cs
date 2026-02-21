using ROGraph.Backend.DataProviders.Interfaces;
using ROGraph.Shared.Enums;
using ROGraph.Shared.Models;

namespace ROGraph.Backend.DataProviders.MockProviders;

public class MockReadingOrderProvider : IReadingOrderProvider
{
    public bool CreateReadingOrder(string name, string? description)
    {
        return true;
    }

    public ReadingOrder? GetReadingOrder(Guid id)
    {
        ReadingOrderContentsManager content = new ReadingOrderContentsManager();
        CoordinateTranslator coordinateTranslator = new CoordinateTranslator(2, 2);

        Guid x1 = coordinateTranslator.GetXFromInt(0);
        Guid x2 = coordinateTranslator.GetXFromInt(1);
        Guid y1 = coordinateTranslator.GetYFromInt(0);
        Guid y2 = coordinateTranslator.GetYFromInt(1);


        Node node1 = new Node(Guid.NewGuid(), "Node 1", Guid.NewGuid(), DateTime.Now, DateTime.Now, x1, y1, NodeType.Diamond);
        Node node2 = new Node(Guid.NewGuid(), "Node 2", Guid.NewGuid(), DateTime.Now, DateTime.Now, x1, y2, NodeType.Diamond);
        Node node3 = new Node(Guid.NewGuid(), "Node 3", Guid.NewGuid(), DateTime.Now, DateTime.Now, x2, y1, NodeType.Diamond);

        content.AddNode(node1);
        content.AddNode(node2);
        content.AddNode(node3);

        Connector c1 = new Connector((x1, y1), (x2, y1));
        Connector c2 = new Connector((x1, y2), (x2, y1));

        content.AddConnector(c1);
        content.AddConnector(c2);

        var mockReadingOrder = new ReadingOrder("Mock Reading Order", Guid.NewGuid(), content, "This is a mock reading order");
        mockReadingOrder.CoordinateTranslator = coordinateTranslator;

        return mockReadingOrder;
    }
    
    public List<ReadingOrderOverview> GetReadingOrders()
    {
        var list = new List<ReadingOrderOverview>
        {
            new ReadingOrderOverview("Reading Order 1", Guid.NewGuid()),
            new ReadingOrderOverview("Reading Order 2", Guid.NewGuid()),
        };

        return list;
    }

    public ReadingOrderOverview? GetReadingOrderOverview(Guid id)
    {
        return new ReadingOrderOverview("Reading Order 1", Guid.NewGuid());
    }
}
