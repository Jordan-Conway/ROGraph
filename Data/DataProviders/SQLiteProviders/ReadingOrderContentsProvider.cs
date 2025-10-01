using ROGraph.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROGraph.Data.DataProviders.SQLiteProviders
{
    public class ReadingOrderContentsProvider
    {
        private static readonly string GET_NODES = 
            "Select nodes.id, name, x, y, type, isCompleted, description, origin, created, lastModified, Checklist " +
            "From @roNum Join nodes on @roNum.id=nodes.id";

        public ReadingOrderContents? GetReadingOrderNodes(ReadingOrderOverview overview, CoordinateTranslator translator)
        {
            return this.GetReadingOrderNodes(overview.Id, translator);
        }

        public ReadingOrderContents? GetReadingOrderNodes(Guid id, CoordinateTranslator translator)
        {
            ReadingOrderContents contents = new ReadingOrderContents();

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

                        Guid? x = translator.GetFromInt(result.GetInt32(2)).Output;
                        Guid? y = translator.GetFromInt(result.GetInt32(3)).Output;

                        if (x == null || y == null)
                        {
                            throw new ArgumentException("Integers out of translation range");
                        }

                        contents.Nodes.Add(new DrawableNode(
                            result.GetGuid(0),
                            result.GetString(1),
                            result.GetGuid(7),
                            result.GetDateTime(8),
                            result.GetDateTime(9),
                            (Guid)x,
                            (Guid)y,
                            (Enums.NodeType)result.GetInt32(4),
                            (result.GetInt32(5) == 0),
                            null,
                            result.GetString(6)));
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Debug.WriteLine("Exception raised while fetching Reading Order Contents");
                Debug.WriteLine(ex.Message);
                return null;
            }
            Debug.WriteLine("Leaving provider with " + contents.Nodes.Count + " nodes");
            return contents;
        }
    }
}
