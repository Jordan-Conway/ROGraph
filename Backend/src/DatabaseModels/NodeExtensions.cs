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
}