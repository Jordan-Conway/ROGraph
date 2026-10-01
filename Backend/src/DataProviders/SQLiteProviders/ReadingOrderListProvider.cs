using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ROGraph.Backend.Context;
using ROGraph.Backend.Contracts;
using ROGraph.Backend.DatabaseModels;
using ROGraph.Backend.Repositories.Nodes;
using ROGraph.Backend.Scripts;
using ROGraph.Shared.Enums;
using ROGraph.Shared.Models;

namespace ROGraph.Backend.DataProviders.SQLiteProviders;

public class ReadingOrderListProvider : IReadingOrderProvider
{
    private static readonly string ConnectionString = "Data Source = " + FilePathProvider.GetDatabaseFilePath();
    
    private IDBContextFactory  _dbContextFactory;

    public ReadingOrderListProvider(IDBContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<ReadingOrderOverview?> GetReadingOrderOverview(Guid id, CancellationToken token = default)
    {
        try
        {
            var context = _dbContextFactory.CreateDbContext();
            
            return context.GetSet<ReadingOrderOverviewDbModel>().FirstOrDefault(o => o.Id == id);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task<bool> UpdateReadingOrderOverview(ReadingOrderOverview readingOrderOverview, CancellationToken token = default)
    {
        try
        {
            var context = _dbContextFactory.CreateDbContext();
            
            var existing = context.GetSet<ReadingOrderOverviewDbModel>().FirstOrDefault(o => o.Id == readingOrderOverview.Id);

            if (existing is null)
            {
                Console.WriteLine($"Reading order with {readingOrderOverview.Id} was not found, it will be created instead");
                return await CreateReadingOrder(readingOrderOverview, token);
            }
            
            var updated = readingOrderOverview.ToDbModel();

            await context.Add(updated, token);

            var rowsChanged = await context.Save(token);

            if (rowsChanged == 0)
            {
                Debug.WriteLine("No rows were updated");
                return false;
            }

            if (rowsChanged > 2)
            {
                Debug.WriteLine("Updated multiple rows, but should have been 1");
            }
            
        }
        catch (SQLiteException ex)
        {
            Debug.WriteLine("Exception while updating overview");
            Debug.WriteLine(ex.Message);
            return false;
        }

        return true;
    }

    public Task<List<ReadingOrderOverview>> GetReadingOrders(CancellationToken token = default)
    {
        try
        {
            var context = _dbContextFactory.CreateDbContext();

            return context.GetSet<ReadingOrderOverviewDbModel>()
                .Where(o => o.Status == ReadingOrderStatus.Active)
                .Select(o => o.ToOverview()).ToListAsync(token);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task<bool> CreateReadingOrder(ReadingOrderOverview overview, CancellationToken token = default)
    {
        try
        {
            var context = _dbContextFactory.CreateDbContext();
            var toCreate = overview.ToDbModel();

            await context.Add(toCreate, token);
            await context.Save(token);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return false;
        }

        return true;
    }

    public async Task<ReadingOrder?> GetReadingOrder(Guid id, CancellationToken token = default)
    {
        var overview = await GetReadingOrderOverview(id) ?? throw new InvalidOperationException($"No reading order with id {id.ToString()}");
        var coordinateTranslator = new CoordinateTranslator(overview.MaxX, overview.MaxY);
        
        try
        {
            using var connection = new SQLiteConnection(ConnectionString);
            connection.Open();

            var nodes = GetReadingOrderNodes(id, coordinateTranslator, connection);
            var connectors = GetReadingOrderConnectors(id, coordinateTranslator, connection);
            
            var readingOrder = new ReadingOrder(
                overview.Name,
                overview.Id,
                new ReadingOrderContentsManager(nodes, connectors),
                overview.Description ?? string.Empty)
            {
                CoordinateTranslator = coordinateTranslator
            };

            return readingOrder;
        }
        catch (SQLiteException ex)
        {
            Debug.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task<bool> UpdateReadingOrder(ReadingOrder readingOrder, CancellationToken token = default)
    {
        try
        {
            var context = _dbContextFactory.CreateDbContext();
            var translator = readingOrder.CoordinateTranslator ??
                             throw new InvalidOperationException(
                                 "Cannot add update reading order without coordinate translator");
            ;

            await using var connection = new SQLiteConnection(ConnectionString);
            connection.Open();

            var nodes = readingOrder.Contents.GetNodes();
            var existingNodes = GetReadingOrderNodes(readingOrder.Id, translator, connection);
            var existingNodeIds = existingNodes.Select(n => n.Id).ToList();
            var nodesToCreate = nodes.Where(n => !existingNodeIds.Contains(n.Id));
            var nodesToUpdate = nodes.Where(n => existingNodeIds.Contains(n.Id));
            var nodesToDelete = existingNodes.Where(n => !nodes.Contains(n, new NodeComparer())).Select(n => n.Id);
            
            await context.GetSet<NodeDbModel>().Where(n => nodesToDelete.Contains(n.Id)).ExecuteDeleteAsync(token);

            foreach (var node in nodesToCreate)
            {
                var x = translator.GetXFromId(node.X);
                var y = translator.GetYFromId(node.Y);

                if (!x.Success || !y.Success)
                {
                    Debug.WriteLine("Cannot save node without x and y coordinates");
                }

                var nodeDbModel = node.ToDbModel();
                var placement = new NodePlacementDbModel
                {
                    ReadingOrderId = readingOrder.Id,
                    NodeId = nodeDbModel.Id,
                    X = x.Output,
                    Y = y.Output
                };

                await context.Add(nodeDbModel, token);
                await context.Add(placement, token);
            }

            foreach (var node in nodesToUpdate)
            {
                var x = translator.GetXFromId(node.X);
                var y = translator.GetYFromId(node.Y);

                if (!x.Success || !y.Success)
                {
                    Debug.WriteLine("Cannot save node without x and y coordinates");
                }

                var nodeDbModel = node.ToDbModel();
                var placement = new NodePlacementDbModel
                {
                    ReadingOrderId = readingOrder.Id,
                    NodeId = nodeDbModel.Id,
                    X = x.Output,
                    Y = y.Output
                };

                var existingNode = await context.GetSet<NodeDbModel>().FirstAsync(n => n.Id == node.Id, token);
                var existingPlacement = await context.GetSet<NodePlacementDbModel>()
                    .FirstAsync(p => p.NodeId == node.Id && p.ReadingOrderId == readingOrder.Id, token);

                existingNode = existingNode with
                {
                    Name = nodeDbModel.Name,
                    ChecklistId = nodeDbModel.ChecklistId,
                    Description = nodeDbModel.Description,
                    IsCompleted = nodeDbModel.IsCompleted,
                    Type = nodeDbModel.Type
                };
                existingPlacement = existingPlacement with
                {
                    X = placement.X,
                    Y = placement.Y
                };

                await context.Save(token);
            }

            var connectors = readingOrder.Contents.GetConnectors();
            var existingConnectors = GetReadingOrderConnectors(readingOrder.Id, translator, connection);
            var connectorComparer = new ConnectorComparer();
            var connectorsToCreate = connectors.Except(existingConnectors, connectorComparer);
            var connectorsToDelete = existingConnectors.Where(x => !connectors.Contains(x, connectorComparer)).Select(c => c.Id);

            await context.GetSet<NodeDbModel>().Where(c => connectorsToDelete.Contains(c.Id)).ExecuteDeleteAsync(token);

            foreach (var connector in connectorsToCreate)
            {
                var x1 = translator.GetXFromId(connector.Origin.Item1);
                var y1 = translator.GetYFromId(connector.Origin.Item2);
                var x2 = translator.GetXFromId(connector.Destination.Item1);
                var y2 = translator.GetYFromId(connector.Destination.Item2);

                var connectorDbModel = new ConnectorDbModel
                {
                    X1 = x1.Success
                        ? x1.Output
                        : throw new InvalidOperationException("Cannot save connector without x1"),
                    Y1 = y1.Success
                        ? y1.Output
                        : throw new InvalidOperationException("Cannot save connector without y1"),
                    X2 = x2.Success
                        ? x2.Output
                        : throw new InvalidOperationException("Cannot save connector withotu x2"),
                    Y2 = y2.Success
                        ? y2.Output
                        : throw new InvalidOperationException("Cannot save connector without y2")
                };
                context.GetSet<ConnectorDbModel>().Add(connectorDbModel);
            }

            await context.Save(token);

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            throw;
        }

        return true;
    }

    public async Task<bool> DeleteReadingOrder(Guid id, CancellationToken token = default)
    {
        try
        {
            var context = new ReadingOrderContext();
            
            var existing = context.ReadingOrderOverviews.FirstOrDefault(x => x.Id == id);

            if (existing is null)
            {
                Console.WriteLine($"Cannot delete  reading order with id {id} as it does not exist");
                return false;
            }

            existing.Status = ReadingOrderStatus.Deleted;

            await context.SaveChangesAsync(token);
        }
        catch (Exception ex )
        {
            Debug.WriteLine(ex.Message);
            return false;
        }

        return true;
    }

    private static List<Node> GetReadingOrderNodes(Guid id, CoordinateTranslator coordinateTranslator, SQLiteConnection connection)
    {
        var getNodesCommand = connection.CreateCommand();
        getNodesCommand.CommandText = ScriptReader.GetReadingOrderNodesScript();
        getNodesCommand.Parameters.Add("@roId", DbType.String).Value = id.ToString();
            
        List<Node> nodes = [];
        var nodesReader =  getNodesCommand.ExecuteReader();
        while (nodesReader.HasRows && nodesReader.Read())
        {
            var guid = nodesReader.GetGuid(0);
            var name = nodesReader.GetString(1);
            var origin = nodesReader.GetGuid(5);
            var created = nodesReader.GetDateTime(7);
            var lastModified = nodesReader.GetDateTime(8);
            var x = nodesReader.GetInt32(11);
            var y = nodesReader.GetInt32(12);
            var typeInt = nodesReader.GetInt32(6);
            var type = (NodeType)typeInt;
            var isCompleted = nodesReader.GetBoolean(3);
            var description = nodesReader.GetString(2);

            nodes.Add(new Node(
                guid,
                name,
                origin,
                created,
                lastModified,
                coordinateTranslator.GetXFromInt(x),
                coordinateTranslator.GetYFromInt(y),
                type,
                isCompleted,
                description: description
            ));
        }

        return nodes;
    }
    
    private static List<Connector> GetReadingOrderConnectors(Guid id, CoordinateTranslator coordinateTranslator, SQLiteConnection connection)
    {
        var context = new ReadingOrderContext();

        var dbConnectors = context.Connectors.Where(c => c.ReadingOrderId == id).ToList();

        return dbConnectors.Select(c =>
        {
            var translatedX1 = coordinateTranslator.GetXFromInt(c.X1);
            var translatedY1 = coordinateTranslator.GetYFromInt(c.Y1);
            var translatedX2 = coordinateTranslator.GetXFromInt(c.X2);
            var translatedY2 = coordinateTranslator.GetYFromInt(c.Y2);

            return new Connector((translatedX1, translatedY1), (translatedX2, translatedY2))
            {
                Id = c.Id
            };
        }).ToList();
    }
}