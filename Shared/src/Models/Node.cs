using System;
using System.Collections.Generic;
using ROGraph.Shared.Enums;

namespace ROGraph.Shared.Models;

public record Node
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public bool IsCompleted { get; set; } = false;
    public Checklist? Checklist { get; set; } = null;
    public Guid Origin { get; set; } = Guid.Empty;
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime LastModified { get; set; }  = DateTime.UtcNow;

    public Guid X { get; set; } = Guid.Empty;
    public Guid Y { get; set; } = Guid.Empty;
    public NodeType Type { get; set; } = NodeType.CIRCLE;
    
    public Node() {}

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