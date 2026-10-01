using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ROGraph.Backend.Contracts;
using ROGraph.Backend.DatabaseModels;
using ROGraph.Backend.Scripts;
using ROGraph.Shared.Enums;
using ROGraph.Shared.Models;

namespace ROGraph.Backend.DataProviders.SQLiteProviders;

public class ReadingOrderListProvider : IReadingOrderProvider
{
    private static readonly string ConnectionString = "Data Source = " + FilePathProvider.GetDatabaseFilePath();

    public ReadingOrderOverview? GetReadingOrderOverview(Guid id)
    {
        try
        {
            using var connection = new SQLiteConnection(ConnectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = ScriptReader.GetAllReadingOrdersScript();
            command.Parameters.Add(new SQLiteParameter("@roId", id.ToString()));
            using var reader = command.ExecuteReader();

            if (reader.HasRows && reader.Read())
            {
                return new ReadingOrderOverview
                (
                    reader.GetString(1),
                    reader.GetGuid(0),
                    reader.GetString(2),
                    reader.GetInt32(3),
                    reader.GetInt32(4)
                );
            }
        }
        catch (SQLiteException ex)
        {
            Debug.WriteLine(ex.Message);
            throw;
        }
        
        return null;
    }

    public bool UpdateReadingOrderOverview(ReadingOrderOverview readingOrderOverview)
    {
        try
        {
            using var connection = new SQLiteConnection(ConnectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = ScriptReader.GetUpdateReadingOrderScript();
            command.Parameters.Add(new SQLiteParameter("@id", readingOrderOverview.Id.ToString()));
            command.Parameters.Add(new SQLiteParameter("@name", readingOrderOverview.Name));
            command.Parameters.Add(new SQLiteParameter("@description", readingOrderOverview.Description ?? string.Empty));

            var rowCount = command.ExecuteNonQuery();

            if (rowCount == 0)
            {
                Debug.WriteLine("No rows were updated");
                return false;
            }

            if (rowCount > 2)
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

    public List<ReadingOrderOverview> GetReadingOrders()
    {
        List<ReadingOrderOverview> overviews = [];
        
        try
        {
            using var connection = new SQLiteConnection(ConnectionString);
            connection.Open();
            var script = ScriptReader.GetAllReadingOrdersScript();
            var command = connection.CreateCommand();
            command.CommandText = script;
            using var reader = command.ExecuteReader();
            
            while(reader.HasRows && reader.Read())
            {
                var overview = new ReadingOrderOverview
                (
                    reader.GetString(1),
                    reader.GetGuid(0),
                    reader.GetString(2),
                    reader.GetInt32(3),
                    reader.GetInt32(4)
                );
                
                overviews.Add(overview);
            }
        }
        catch (SQLiteException ex)
        {
            Debug.WriteLine(ex.Message);
        }

        return overviews;
    }

    public bool CreateReadingOrder(ReadingOrderOverview overview)
    {
        try
        {
            var connection = new SQLiteConnection(ConnectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = ScriptReader.CreateReadingOrderScript();
            command.Parameters.Add(new SQLiteParameter("@id", overview.Id == Guid.Empty ? overview.ToString() : Guid.NewGuid().ToString()));
            command.Parameters.Add(new SQLiteParameter("@name", overview.Name));
            command.Parameters.Add(new SQLiteParameter("@description", overview.Description ?? string.Empty));
            command.Parameters.Add(new SQLiteParameter("@maxX", value: 0));
            command.Parameters.Add(new SQLiteParameter("@maxY", value: 0));
            
            command.ExecuteNonQuery();
        }
        catch (SQLiteException ex)
        {
            Debug.WriteLine(ex.Message);
            return false;
        }

        return true;
    }

    public ReadingOrder GetReadingOrder(Guid id)
    {
        var overview = GetReadingOrderOverview(id) ?? throw new InvalidOperationException($"No reading order with id {id.ToString()}");
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

    public async Task<bool> UpdateReadingOrder(ReadingOrder readingOrder, CancellationToken token)
    {
        try
        {
            var context = new ReadingOrderContext();
            var translator = readingOrder.CoordinateTranslator ??
                             throw new InvalidOperationException(
                                 "Cannot add update reading order without coordinate translator");
            ;

            await using var connection = new SQLiteConnection(ConnectionString);
            connection.Open();

            var nodes = readingOrder.Contents.GetNodes();
            var existingNodes = GetReadingOrderNodes(readingOrder.Id, translator, connection);
            var nodesToDelete = existingNodes.Where(n => !nodes.Contains(n, new NodeComparer()));

            foreach (var node in nodesToDelete)
            {
                var deleteNodeCommand = connection.CreateCommand();
                deleteNodeCommand.CommandText = ScriptReader.GetDeleteNodeScript();
                deleteNodeCommand.Parameters.Add(new SQLiteParameter("@nodeId", node.Id.ToString()));
                deleteNodeCommand.Parameters.Add(new SQLiteParameter("@roId", readingOrder.Id.ToString()));

                deleteNodeCommand.ExecuteNonQuery();
            }

            foreach (var node in nodes)
            {
                var x = translator.GetXFromId(node.X);
                var y = translator.GetYFromId(node.Y);

                if (!x.Success || !y.Success)
                {
                    Debug.WriteLine("Cannot save node with x and y coordinates");
                }

                var addNodeCommand = connection.CreateCommand();
                addNodeCommand.CommandText = ScriptReader.GetAddNodeScript();
                addNodeCommand.Parameters.Add(new SQLiteParameter("@nodeId", node.Id.ToString()));
                addNodeCommand.Parameters.Add(new SQLiteParameter("@name", node.Name));
                addNodeCommand.Parameters.Add(new SQLiteParameter("@description", node.Description));
                addNodeCommand.Parameters.Add(new SQLiteParameter("@isCompleted", node.IsCompleted));
                addNodeCommand.Parameters.Add(new SQLiteParameter("@checkListId", null));
                addNodeCommand.Parameters.Add(new SQLiteParameter("@origin", node.Origin.ToString()));
                addNodeCommand.Parameters.Add(new SQLiteParameter("@type", node.Type));
                addNodeCommand.Parameters.Add(new SQLiteParameter("@readingOrderId", readingOrder.Id.ToString()));
                addNodeCommand.Parameters.Add(new SQLiteParameter("@x", x.Output));
                addNodeCommand.Parameters.Add(new SQLiteParameter("@y", y.Output));

                addNodeCommand.ExecuteNonQuery();
            }

            var connectors = readingOrder.Contents.GetConnectors();
            var existingConnectors = GetReadingOrderConnectors(readingOrder.Id, translator, connection);
            var connectorComparer = new ConnectorComparer();
            var connectorsToCreate = connectors.Except(existingConnectors, connectorComparer);
            var connectorsToDelete = existingConnectors.Where(x => !connectors.Contains(x, connectorComparer)).Select(c => c.Id);

            await context.Connectors.Where(c => connectorsToDelete.Contains(c.Id)).ExecuteDeleteAsync(token);

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
                context.Connectors.Add(connectorDbModel);
            }

            await context.SaveChangesAsync(token);

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            throw;
        }

        return true;
    }

    public bool DeleteReadingOrder(Guid id)
    {
        if (id == Guid.Empty)
        {
            return false;
        }

        try
        {
            using var connection = new SQLiteConnection(ConnectionString);
            connection.Open();
            
            var command = connection.CreateCommand();
            command.CommandText = ScriptReader.DeleteReadingOrderScript();
            command.Parameters.Add(new SQLiteParameter("@id", id.ToString()));
            
            var rowsUpdated = command.ExecuteNonQuery();

            switch (rowsUpdated)
            {
                case 0: Debug.WriteLine($"Tried to delete reading order with id {id.ToString()}, but it was not found"); break;
                case 1: break;
                default: Debug.WriteLine($"Deleted multiple reading orders with  id {id.ToString()}"); break;
            }
        }
        catch (SQLiteException ex )
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