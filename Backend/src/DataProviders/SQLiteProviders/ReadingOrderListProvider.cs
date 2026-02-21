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

    public bool CreateReadingOrder(string name, string? description)
    {
        throw new NotImplementedException();
    }

    public ReadingOrder? GetReadingOrder(Guid id)
    {
        var overview = GetReadingOrderOverview(id) ?? throw new InvalidOperationException($"No reading order with id {id.ToString()}");
        var coordinateTranslator = new CoordinateTranslator(overview.MaxX, overview.MaxY);
        
        try
        {
            using var connection = new SQLiteConnection(ConnectionString);
            connection.Open();
        
            var getNodesCommand = connection.CreateCommand();
            getNodesCommand.CommandText = ScriptReader.GetReadingOrderNodesScript();
            getNodesCommand.Parameters.Add("@roId", DbType.String).Value = id.ToString();
            
            List<Node> nodes = [];
            List<Connector> connectors = [];
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
            
            var readingOrder = new ReadingOrder(
                overview.Name,
                overview.Id,
                new ReadingOrderContentsManager(nodes, connectors),
                overview.Description ?? string.Empty);
            readingOrder.CoordinateTranslator = coordinateTranslator;

            return readingOrder;
        }
        catch (SQLiteException ex)
        {
            Debug.WriteLine(ex.Message);
            throw;
        }
    }
}