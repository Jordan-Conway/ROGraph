using System;
using ROGraph.Shared.Models;

namespace ROGraph.Backend.DatabaseModels;

internal static class NodeExtensions
{
    public static NodeDbModel ToDbModel(this Node node)
    {
        return new NodeDbModel
        {
            Id = node.Id,
            Name = node.Name,
            Description = node.Description ?? string.Empty,
            IsCompleted = node.IsCompleted,
            Origin = node.Origin,
            Type = node.Type,
            Created = node.Created,
            LastModified = node.LastModified,
        };
    }

    public static Node ToNode(this NodeDbModel model, Guid? X = null, Guid? Y = null)
    {
        return new Node
        (
            model.Id,
            model.Name,
            model.Origin,
            model.Created,
            model.LastModified,
            X ?? Guid.Empty,
            Y ?? Guid.Empty,
            model.Type,
            model.IsCompleted,
            description: model.Description
        );
    }
}
