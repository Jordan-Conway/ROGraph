using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ROGraph.Backend.Context;
using ROGraph.Backend.DatabaseModels;
using ROGraph.Shared.Models;

namespace ROGraph.Backend.Repositories.Nodes;

internal class NodeRepository : INodeRepository
{
    private readonly IDBContextFactory _dbContextFactory;

    public NodeRepository(IDBContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<IList<Node>> GetNodesForReadingOrder(Guid readingOrderId, CoordinateTranslator translator, CancellationToken token)
    {
        var context = _dbContextFactory.CreateDbContext();

        var nodes = context.GetSet<NodeDbModel>().Join(
                context.GetSet<NodePlacementDbModel>(),
                n => n.Id,
                p => p.NodeId,
                (node, placement) => new { Node = node, Placement = placement })
            .Where(n => n.Placement.ReadingOrderId == readingOrderId)
            .Select(n => n.Node.ToNode(translator.GetXFromInt(n.Placement.X), translator.GetYFromInt(n.Placement.Y)));

        return await nodes.ToListAsync(token);
    }

    public async Task<Guid> CreateNode(Node node, Guid readingOrderId, (int X, int Y) placement,
        CancellationToken token)
    {
        var context = _dbContextFactory.CreateDbContext();

        if (node.Id == Guid.Empty)
        {
            node.Id = Guid.NewGuid();
        }

        var alreadyExists = await context.GetSet<NodeDbModel>().FirstOrDefaultAsync(n => n.Id == node.Id, token) is not null;

        if (!alreadyExists)
        {
            if (node.Origin == Guid.Empty)
            {
                node.Origin = readingOrderId;
            }

            node.Created = DateTime.UtcNow;
            node.LastModified = node.Created;

            var nodeToCreate = node.ToDbModel();
            await context.Add(nodeToCreate, token);
        }

        await CreatePlacement(node.Id, readingOrderId, placement, token);
        await context.Save(token);

        return node.Id;
    }

    public async Task<bool> UpdateNode(Node node, Guid readingOrderId, (int X, int Y) placement, CancellationToken token)
    {
        var context = _dbContextFactory.CreateDbContext();

        var existingNode = await context.GetSet<NodeDbModel>().FirstOrDefaultAsync(n => n.Id == node.Id, token);

        if (existingNode is null)
        {
            Console.WriteLine($"Cannot update node with id {node.Id} as it does not exist");
            return false;
        }

        existingNode.Name = node.Name;
        existingNode.Type = node.Type;
        existingNode.Description = node.Description ?? string.Empty;
        existingNode.IsCompleted = node.IsCompleted;
        existingNode.ChecklistId = node.Checklist?.Id ?? Guid.Empty;

        var existingPlacement = await context.GetSet<NodePlacementDbModel>()
            .FirstOrDefaultAsync(p => p.ReadingOrderId == readingOrderId && p.NodeId == node.Id, token);

        if (existingPlacement is null)
        {
            existingPlacement = await CreatePlacement(node.Id, readingOrderId, placement, token);
        }
        else
        {
            existingPlacement.X = placement.X;
            existingPlacement.Y = placement.Y;
        }

        var rowsChanged = await context.Save(token);

        return rowsChanged > 0;
    }

    private async Task<NodePlacementDbModel> CreatePlacement(Guid nodeId, Guid readingOrderId, (int X, int Y) placement, CancellationToken token)
    {
        var context = _dbContextFactory.CreateDbContext();

        var placementModel = new NodePlacementDbModel
        {
            Id = Guid.NewGuid(),
            NodeId = nodeId,
            ReadingOrderId = readingOrderId,
            X = placement.X,
            Y = placement.Y,
        };

        await context.Add(placementModel, token);

        await context.Save(token);

        return placementModel;
    }
}
