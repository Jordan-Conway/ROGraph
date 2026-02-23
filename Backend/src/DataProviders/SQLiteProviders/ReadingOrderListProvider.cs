using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using ROGraph.Backend.DataProviders.Interfaces;
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

    public bool UpdateReadingOrder(ReadingOrder readingOrder)
    {
        try
        {
            using var connection = new SQLiteConnection(ConnectionString);
            connection.Open();

            var nodes = readingOrder.Contents.GetNodes();
            var existingNodes = GetReadingOrderNodes(readingOrder.Id, readingOrder.CoordinateTranslator!, connection);
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
                var x = readingOrder.CoordinateTranslator?.GetXFromId(node.X) ?? throw new InvalidOperationException("Cannot add node without translator");
                var y = readingOrder.CoordinateTranslator?.GetYFromId(node.Y) ?? throw new InvalidOperationException("Cannot add node without translator");

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
            var existingConnectors = GetReadingOrderConnectors(readingOrder.Id, readingOrder.CoordinateTranslator!, connection);
            var connectorsToDelete = existingConnectors.Where(x => !connectors.Contains(x, new ConnectorComparer()));

            foreach (var connector in connectorsToDelete)
            {
                var x1 = readingOrder.CoordinateTranslator?.GetXFromId(connector.Origin.Item1)  ?? throw new InvalidOperationException("Cannot save connector without translator");
                var y1 = readingOrder.CoordinateTranslator?.GetYFromId(connector.Origin.Item2)  ?? throw new InvalidOperationException("Cannot save connector without translator");
                var x2 = readingOrder.CoordinateTranslator?.GetXFromId(connector.Destination.Item1) ??  throw new InvalidOperationException("Cannot save connector without translator");
                var y2 = readingOrder.CoordinateTranslator?.GetYFromId(connector.Destination.Item2) ?? throw new InvalidOperationException("Cannot save connector without translator");
                
                var deleteConnectorCommand = connection.CreateCommand();
                deleteConnectorCommand.CommandText = ScriptReader.GetDeleteConnectorScript();
                deleteConnectorCommand.Parameters.Add(new SQLiteParameter("@x1", x1.Success ? x1.Output : throw new  InvalidOperationException("Cannot delete connector without x1")));
                deleteConnectorCommand.Parameters.Add(new SQLiteParameter("@y1", y1.Success ? y1.Output : throw new  InvalidOperationException("Cannot delete connector without y1")));
                deleteConnectorCommand.Parameters.Add(new SQLiteParameter("@x2", x2.Success ? x2.Output : throw new  InvalidOperationException("Cannot delete connector without x2")));
                deleteConnectorCommand.Parameters.Add(new SQLiteParameter("@y2", y2.Success ? y2.Output : throw new  InvalidOperationException("Cannot delete connector without y2")));
                deleteConnectorCommand.Parameters.Add(new SQLiteParameter("@roId", readingOrder.Id.ToString()));
                
                deleteConnectorCommand.ExecuteNonQuery();
            }
            
            foreach (var connector in connectors)
            {
                var x1 = readingOrder.CoordinateTranslator?.GetXFromId(connector.Origin.Item1)  ?? throw new InvalidOperationException("Cannot save connector without translator");
                var y1 = readingOrder.CoordinateTranslator?.GetYFromId(connector.Origin.Item2)  ?? throw new InvalidOperationException("Cannot save connector without translator");
                var x2 = readingOrder.CoordinateTranslator?.GetXFromId(connector.Destination.Item1) ??  throw new InvalidOperationException("Cannot save connector without translator");
                var y2 = readingOrder.CoordinateTranslator?.GetYFromId(connector.Destination.Item2) ?? throw new InvalidOperationException("Cannot save connector without translator");
                
                var addConnectorsCommand = connection.CreateCommand();
                addConnectorsCommand.CommandText = ScriptReader.GetAddConnectorScript();
                addConnectorsCommand.Parameters.Add(new SQLiteParameter("@x1", x1.Success ? x1.Output : throw new  InvalidOperationException("Cannot save connector without x1")));
                addConnectorsCommand.Parameters.Add(new SQLiteParameter("@y1", y1.Success ? y1.Output : throw new  InvalidOperationException("Cannot save connector without y1")));
                addConnectorsCommand.Parameters.Add(new SQLiteParameter("@x2", x2.Success ? x2.Output : throw new  InvalidOperationException("Cannot save connector without x2")));
                addConnectorsCommand.Parameters.Add(new SQLiteParameter("@y2", y2.Success ? y2.Output : throw new  InvalidOperationException("Cannot save connector without y2")));
                addConnectorsCommand.Parameters.Add(new SQLiteParameter("@roId", readingOrder.Id.ToString()));
                
                addConnectorsCommand.ExecuteNonQuery();
            }

        }
        catch (SQLiteException ex)
        {
            Debug.WriteLine(ex.Message);
            throw;
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
        var getConnectorsCommand = connection.CreateCommand();
        getConnectorsCommand.CommandText = ScriptReader.GetReadingOrderConnectorsScript();
        getConnectorsCommand.Parameters.Add("@roId", DbType.String).Value = id.ToString();
            
        List<Connector> connectors = [];
        var reader = getConnectorsCommand.ExecuteReader();
        
        while (reader.HasRows && reader.Read())
        {
            var x1 = reader.GetInt32(0);
            var y1 = reader.GetInt32(1);
            var x2 = reader.GetInt32(2);
            var y2 = reader.GetInt32(3);
                
            connectors.Add(new Connector(
                (coordinateTranslator.GetXFromInt(x1), coordinateTranslator.GetYFromInt(y1)),
                (coordinateTranslator.GetXFromInt(x2), coordinateTranslator.GetYFromInt(y2))
            ));
        }
        
        return connectors;
    }
}