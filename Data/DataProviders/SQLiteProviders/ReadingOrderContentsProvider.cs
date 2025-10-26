using ROGraph.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ROGraph.Data.DataProviders.SQLiteProviders
{
    public class ReadingOrderContentsProvider
    {
        private static readonly string GET_NODES = 
            "Select nodes.id, name, x, y, type, isCompleted, description, origin, created, lastModified, Checklist " +
            "From @roNum Join nodes on @roNum.id=nodes.id";

        private static readonly string GET_CONNECTORS =
            "Select * from @roNum";

        public ReadingOrderContents? GetReadingOrderContents(ReadingOrderOverview overview, CoordinateTranslator translator)
        {
            return this.GetReadingOrderContents(overview.Id, translator);
        }

        public ReadingOrderContents? GetReadingOrderContents(Guid id, CoordinateTranslator translator)
        {
            List<Node> nodes = this.GetReadingOrderNodes(id, translator);
            List<Connector> connectors = this.GetReadingOrderConnectors(id, translator);

            return new ReadingOrderContents(nodes, connectors);
        }

        private List<Node> GetReadingOrderNodes(Guid id, CoordinateTranslator translator)
        {
            List<Node> nodes = new List<Node>();

            string dataFolder = DataFolderProvider.GetDataDirectory();
            string connectionString = "Data Source=" + dataFolder + "/SaveData/Nodes.db;Version=3";
            Debug.WriteLine(connectionString);
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    SQLiteCommand cmd = connection.CreateCommand();
                    cmd.CommandText = GET_NODES.Replace("@roNum", ("'" + id.ToString() + "'"));
                    cmd.CommandType = System.Data.CommandType.Text;
                    Debug.WriteLine(cmd.CommandText);
                    SQLiteDataReader result = cmd.ExecuteReader();
                    while (result.Read())
                    {
                        Debug.WriteLine("Processing result");

                        Guid x = translator.GetXFromInt(result.GetInt32(2));
                        Guid y = translator.GetYFromInt(result.GetInt32(3));

                        nodes.Add(new DrawableNode(
                            result.GetGuid(0),
                            result.GetString(1),
                            result.GetGuid(7),
                            result.GetDateTime(8),
                            result.GetDateTime(9),
                            x,
                            y,
                            (Enums.NodeType)result.GetInt32(4),
                            (result.GetInt32(5) != 0),
                            null,
                            result.GetString(6)));
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Debug.WriteLine("Exception raised while fetching Reading Order Contents");
                Debug.WriteLine(ex.Message);
                return new List<Node>();
            }
            Debug.WriteLine("Leaving provider with " + nodes.Count + " nodes");
            return nodes;
        }

        private List<Connector> GetReadingOrderConnectors(Guid id, CoordinateTranslator translator)
        {
            List<Connector> connectors = new List<Connector>();

            string dataFolder = DataFolderProvider.GetDataDirectory();
            string connectionString = "Data Source=" + dataFolder + "/SaveData/Connectors.db;Version=3";

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    SQLiteCommand cmd = connection.CreateCommand();
                    cmd.CommandText = GET_CONNECTORS.Replace("@roNum", ("'" + id.ToString() + "'"));
                    cmd.CommandType = System.Data.CommandType.Text;
                    Debug.WriteLine(cmd.CommandText);
                    SQLiteDataReader result = cmd.ExecuteReader();
                    while (result.Read())
                    {
                        Debug.WriteLine("Processing result");

                        Guid x1 = translator.GetXFromInt(result.GetInt32(0));
                        Guid y1 = translator.GetYFromInt(result.GetInt32(1));
                        Guid x2 = translator.GetXFromInt(result.GetInt32(2));
                        Guid y2 = translator.GetYFromInt(result.GetInt32(3));

                        connectors.Add(new Connector((x1, y1), (x2, y2)));
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Debug.WriteLine("Exception raised while fetching Reading Order Contents");
                Debug.WriteLine(ex.Message);
                return new List<Connector>();
            }
            Debug.WriteLine("Leaving provider with " + connectors.Count + " nodes");
            return connectors;
        }
    }
}
