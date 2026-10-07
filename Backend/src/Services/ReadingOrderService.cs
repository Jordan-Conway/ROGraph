using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ROGraph.Backend.Contracts;
using ROGraph.Backend.Repositories.Connectors;
using ROGraph.Backend.Repositories.Nodes;
using ROGraph.Backend.Repositories.ReadingOrders;
using ROGraph.Shared.Extensions;
using ROGraph.Shared.Models;

namespace ROGraph.Backend.Services;

internal class ReadingOrderService : IReadingOrderService
{
    private readonly IReadingOrderRepository _readingOrderRepository;
    private readonly INodeRepository _nodeRepository;
    private readonly IConnectorRepository _connectorRepository;

    public ReadingOrderService(IReadingOrderRepository readingOrderRepository, INodeRepository nodeRepository, IConnectorRepository connectorRepository)
    {
        _readingOrderRepository = readingOrderRepository;
        _nodeRepository = nodeRepository;
        _connectorRepository = connectorRepository;
    }

    public async Task<bool> CreateReadingOrder(ReadingOrderOverview overview, CancellationToken token = default)
    {
        return await _readingOrderRepository.CreateReadingOrder(overview, token);
    }

    public async Task<bool> DeleteReadingOrder(Guid id, CancellationToken token = default)
    {
        return await _readingOrderRepository.DeleteReadingOrder(id, token);
    }

    public async Task<ReadingOrder?> GetReadingOrder(Guid id, CancellationToken token = default)
    {
        var overview = await _readingOrderRepository.GetReadingOrder(id, token);

        if (overview is null)
        {
            return null;
        }

        var translator = new CoordinateTranslator(overview.MaxX, overview.MaxY);

        var nodes = await _nodeRepository.GetNodesForReadingOrder(id, translator, token);
        var connectors = await _connectorRepository.GetConnectorsForReadingOrder(id, translator, token);

        var contentManager = new ReadingOrderContentsManager(nodes, connectors);
        var readingOrder = new ReadingOrder(overview.Name, overview.Id, contentManager, overview.Description ?? "");
        readingOrder.CoordinateTranslator = translator;

        return readingOrder;
    }

    public async Task<ReadingOrderOverview?> GetReadingOrderOverview(Guid id, CancellationToken token = default)
    {
        return await _readingOrderRepository.GetReadingOrder(id, token);
    }

    public async Task<IList<ReadingOrderOverview>> GetReadingOrderOverviews(CancellationToken token = default)
    {
        return await _readingOrderRepository.GetAllReadingOrders(token);
    }

    public async Task<bool> UpdateReadingOrder(ReadingOrder readingOrder, CancellationToken token = default)
    {
        var coordinateTranslator = readingOrder.CoordinateTranslator;

        if (coordinateTranslator is null)
        {
            Debug.WriteLine("Cannot update reading order with a coordinate translator");
            return false;
        }

        var existingNodes = (await _nodeRepository.GetNodesForReadingOrder(readingOrder.Id, coordinateTranslator, token))
            .Select(n => n.Id).ToHashSet();
        var existingConnectors = (await _connectorRepository.GetConnectorsForReadingOrder(readingOrder.Id, coordinateTranslator, token))
            .Select(c => c.Id).ToHashSet();

        try
        {
            var nodes = readingOrder.Contents.GetNodes();
            
            foreach (var node in nodes)
            {
                var placement = (coordinateTranslator.GetXFromId(node.X).Output,
                    coordinateTranslator.GetYFromId(node.Y).Output);

                if (existingNodes.Contains(node.Id))
                {
                   await _nodeRepository.UpdateNode(node, readingOrder.Id, placement, token);
                }
                else
                {
                    await _nodeRepository.CreateNode(node, readingOrder.Id, placement, token);
                }
            }

            var nodesToDelete = existingNodes.Where(nodeId => nodes.All(node => node.Id != nodeId));
            await _nodeRepository.DeleteNodePlacements(nodesToDelete, readingOrder.Id, token);

            var connectors = readingOrder.Contents.GetConnectors();
            
            foreach (var connector in connectors)
            {
                if (existingConnectors.Contains(connector.Id))
                {
                    await _connectorRepository.UpdateConnector(connector, coordinateTranslator, token);
                }
                else
                {
                    await _connectorRepository.CreateConnector(connector, readingOrder.Id, coordinateTranslator, token);
                }
            }
            
            var connectorsToDelete = existingConnectors.Where(connectorId => connectors.All(c => c.Id != connectorId));
            await _connectorRepository.DeleteConnectors(connectorsToDelete, token);
            
            await _readingOrderRepository.UpdateReadingOrder(readingOrder.ToOverview(), token);

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    public async Task<bool> UpdateReadingOrderOverview(ReadingOrderOverview readingOrderOverview, CancellationToken token = default)
    {
        return await _readingOrderRepository.UpdateReadingOrder(readingOrderOverview, token);
    }
}
