using ROGraph.Shared.Models;
using ROGraph.Shared.Enums;

namespace tests;

public class ReadingOrderContentsTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestAddNode()
    {
        Guid id = Guid.NewGuid();
        Node node = new Node(id, "New Node", Guid.NewGuid(), DateTime.Now, DateTime.Now, Guid.NewGuid(), Guid.NewGuid(), NodeType.Triangle);
        ReadingOrderContents contents = new ReadingOrderContents();
        contents.Nodes.Add(node);
        Assert.That(node.Equals(contents.GetNode(id)));
    }
}