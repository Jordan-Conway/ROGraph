using ROGraph.Shared.Enums;
using ROGraph.Shared.Models;

namespace ROGraph.Shared.Tests;

public class ReadingOrderContentsTests
{
    private ReadingOrderContentsManager _contentsManager;

    [SetUp]
    public void Setup()
    {
        _contentsManager = new ReadingOrderContentsManager();
    }

    [Test]
    public void TestAddNode()
    {
        Node node = CreateNode();
        _contentsManager.AddNode(node);
        Assert.That(node, Is.EqualTo(_contentsManager.GetNode(node.Id)));
    }

    [Test]
    public void TestAddConnector()
    {
        Connector connector = CreateConnector();
        _contentsManager.AddConnector(connector);

        Assert.That(_contentsManager.GetConnectorsBetweenPositions(connector.Origin).ToArray(), Has.Length.EqualTo(1));
    }

    [Test]
    public void TestDeleteNode()
    {
        Node node1 = CreateNode();
        Node node2 = CreateNode();
        Node node3 = CreateNode();
        Connector connector1 = CreateConnector((node1.GetPosition(), node2.GetPosition()));
        Connector connector2 = CreateConnector((node2.GetPosition(), node3.GetPosition()));

        ReadingOrderContentsManager contentsManager = new ReadingOrderContentsManager();
        contentsManager.AddNode(node1);
        contentsManager.AddNode(node2);
        contentsManager.AddConnector(connector1);
        contentsManager.AddConnector(connector2);

        contentsManager.DeleteNode(node1);

        List<Connector> connectors = [.. contentsManager.GetConnectorsBetweenPositions(node1.GetPosition())];

        Assert.Multiple(() =>
        {
            Assert.That(contentsManager.GetNode(node1.Id), Is.EqualTo(null));
            Assert.That(connectors, Is.Empty);
        });
    }

    [Test]
    public void TestDeleteConnector()
    {
        Connector connector = CreateConnector();
        _contentsManager.AddConnector(connector);
        _contentsManager.DeleteConnector(connector.Origin, connector.Destination);

        Assert.That(_contentsManager.GetConnectorsBetweenPositions(connector.Origin).ToArray(), Is.Empty);
    }

    [Test]
    public void TestReplaceNode()
    {
        Node node1 = CreateNode();
        Node node2 = CreateNode();
        node2.Id = node1.Id;

        _contentsManager.AddNode(node1);
        _contentsManager.ReplaceNode(node2);

        Assert.Multiple(() =>
        {
            Assert.That(_contentsManager.GetNode(node1.Id), Is.EqualTo(node2));
            Assert.That(_contentsManager.GetNode(node1.Id), Is.Not.EqualTo(node1));
        });
    }

    private static Node CreateNode()
    {
        Guid id = Guid.NewGuid();
        Node node = new Node(id, "New Node", Guid.NewGuid(), DateTime.Now, DateTime.Now, Guid.NewGuid(), Guid.NewGuid(), NodeType.TRIANGLE);

        return node;
    }

    private static Connector CreateConnector(((Guid, Guid), (Guid, Guid))? coordinates = null)
    {
        ((Guid, Guid), (Guid, Guid)) points = coordinates ?? ((Guid.NewGuid(), Guid.NewGuid()), (Guid.NewGuid(), Guid.NewGuid()));

        return new Connector(points.Item1, points.Item2);
    }
}