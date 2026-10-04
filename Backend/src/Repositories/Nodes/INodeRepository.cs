using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ROGraph.Shared.Models;

namespace ROGraph.Backend.Repositories.Nodes;

internal interface INodeRepository
{
    public Task<IList<Node>> GetNodesForReadingOrder(Guid readingOrderId, CoordinateTranslator translator, CancellationToken token);

    public Task<Guid> CreateNode(Node node, Guid readingOrderId, (int X, int Y) placement,
        CancellationToken token);

    public Task<bool> UpdateNode(Node node, Guid readingOrderId, (int X, int Y) placement, CancellationToken token);
}
