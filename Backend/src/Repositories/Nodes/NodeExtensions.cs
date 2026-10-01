using System;
using ROGraph.Backend.DatabaseModels;
using ROGraph.Shared.Models;

namespace ROGraph.Backend.Repositories.Nodes;

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

    public static Node ToNode(this NodeDbModel model)
    {
        return new Node
        (
            model.Id,
            model.Name,
            model.Origin,
            model.Created,
            model.LastModified,
            Guid.Empty,
            Guid.Empty,
            model.Type,
            model.IsCompleted,
            description: model.Description
        );
    }
}