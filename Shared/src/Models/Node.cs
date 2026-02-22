using ROGraph.Shared.Enums;

namespace ROGraph.Shared.Models;

public class Node
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    public Checklist? Checklist { get; set; }
    public Guid Origin { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastModified { get; set; }

    public Guid X { get; set; }
    public Guid Y { get; set; }
    public NodeType Type { get; set; }

    public Node(
        Guid id,
        string name, 
        Guid origin, 
        DateTime created, 
        DateTime lastModified,
        Guid x,
        Guid y,
        NodeType nodeType,
        bool isCompleted = false,
        Checklist? checklist = null,
        string? description = null
    )
    {
        Id = id;
        Name = name;
        Origin = origin;
        Created = created;
        LastModified = lastModified;
        X = x;
        Y = y;
        Type = nodeType;
        IsCompleted = isCompleted;
        Checklist = checklist;
        Description = description;
    }

    public (Guid, Guid) GetPosition()
    {
        return (X, Y);
    }
}

public class NodeComparer : IEqualityComparer<Node>
{
    public bool Equals(Node? x, Node? y)
    {
        if (ReferenceEquals(x, y)) return true;

        if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;

        return x.Id == y.Id;
    }
    
    public int GetHashCode(Node node)
    {
        return ReferenceEquals(node, null) ? 0 : node.Id.GetHashCode();
    }
}