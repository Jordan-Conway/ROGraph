using ROGraph.Data.DataProviders.Interfaces;
using ROGraph.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROGraph.Data.DataProviders.MockProviders
{
    public class MockReadingOrderProvider : IReadingOrderProvider
    {
        public bool CreateReadingOrder(string name, string? description)
        {
            return true;
        }

        public ReadingOrder? GetReadingOrder(ReadingOrderOverview overview)
        {
            ReadingOrderContents content = new ReadingOrderContents();
            CoordinateTranslator coordinateTranslator = new CoordinateTranslator(2, 2);

            Guid x1 = coordinateTranslator.GetXFromInt(0).Output;
            Guid x2 = coordinateTranslator.GetXFromInt(1).Output;
            Guid y1 = coordinateTranslator.GetYFromInt(0).Output;
            Guid y2 = coordinateTranslator.GetYFromInt(1).Output;


            Node node1 = new DrawableNode(new Guid(), "Node 1", new Guid(), DateTime.Now, DateTime.Now, x1, y1, Enums.NodeType.Diamond);
            Node node2 = new DrawableNode(new Guid(), "Node 2", new Guid(), DateTime.Now, DateTime.Now, x1, y2, Enums.NodeType.Diamond);
            Node node3 = new DrawableNode(new Guid(), "Node 3", new Guid(), DateTime.Now, DateTime.Now, x2, y1, Enums.NodeType.Diamond);

            content.Nodes.Add(node1);
            content.Nodes.Add(node2);

            Connector c1 = new Connector((x1, y1), (x2, y1));
            Connector c2 = new Connector((x1, y2), (x2, y1));

            content.Connectors.Add(c1);
            content.Connectors.Add(c2);

            ReadingOrder mockReadingOrder = new ReadingOrder("Mock Reading Order", new Guid(), content, "This is a mock reading order");
            mockReadingOrder.CoordinateTranslator = coordinateTranslator;

            return mockReadingOrder;
        }
    }
}
