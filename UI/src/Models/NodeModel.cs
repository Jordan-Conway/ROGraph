using ROGraph.Shared.Models;

namespace ROGraph.UI.Models;

internal partial class NodeModel
{
    public Node Node { get;}
    public int X {get;}
    public int Y {get;}

    public NodeModel(Node node, int x, int y)
    {
        Node = node;
        X = x;
        Y = y;
    }
}