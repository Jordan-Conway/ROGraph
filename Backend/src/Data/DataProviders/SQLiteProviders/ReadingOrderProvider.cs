using ROGraph.Backend.Data.DataProviders;
using ROGraph.Backend.Data.DataProviders.Interfaces;
using ROGraph.Backend.Data.DataProviders.SQLiteProviders;
using ROGraph.Shared.Models;
using System.Data.SQLite;
using System.Diagnostics;

namespace ROGraph.Data.DataProviders.SQLiteProviders
{
    public class ReadingOrderProvider: IReadingOrderProvider
    {
        private static string _dataFolderPath = DataFolderProvider.GetDataDirectory();

        private static readonly string CREATE_NODE_TABLE = "Create Table If Not Exists nodes (id Integer Primary Key, name Text, x Number, y Number)";
        private static readonly string SELECT_NODE = "Select * From Nodes";

        public bool CreateReadingOrder(string name, string? description = "")
        {
            DirectoryInfo currentDirectory = new DirectoryInfo(_dataFolderPath).GetDirectories("ReadingOrders")[0];

            Guid readingOrderId = Guid.NewGuid();
            currentDirectory.CreateSubdirectory(readingOrderId.ToString());

            currentDirectory = currentDirectory.GetDirectories(readingOrderId.ToString())[0];

            bool createdInfoFile = CreateInfoFile(currentDirectory, new ReadingOrderOverview(name, readingOrderId, description));
            bool createdDatabase = CreateDatabase(readingOrderId);

            return createdInfoFile && createdDatabase;
        }

        private static bool CreateInfoFile(DirectoryInfo directory, ReadingOrderOverview readingOrderOverview)
        {
            try
            {
                string FilePath = directory.FullName + "/" + "info.json";
                File.WriteAllText(FilePath, System.Text.Json.JsonSerializer.Serialize(readingOrderOverview, readingOrderOverview.GetType()));
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        private static bool CreateDatabase(Guid guid)
        {
            string directory = _dataFolderPath + "/ReadingOrders/" + guid.ToString() + "/";
            string connectionString = "Data Source=" + directory + guid.ToString() + ".db";

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        SQLiteCommand createNodeTableCommand = new SQLiteCommand(CREATE_NODE_TABLE, connection);
                        createNodeTableCommand.ExecuteNonQuery();
                        Debug.WriteLine("Database Created");
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Failed to Create Database");
                        Debug.WriteLine(ex.Message);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        public ReadingOrder? GetReadingOrder(ReadingOrderOverview overview)
        {
            CoordinateTranslator coordinateTranslator = new CoordinateTranslator(overview.MaxX, overview.MaxY);

            ReadingOrderContentsProvider contentsProvider = new ReadingOrderContentsProvider();
            ReadingOrderContentsManager? contents = contentsProvider.GetReadingOrderContents(overview.Id, coordinateTranslator);

            ReadingOrder readingOrder = new ReadingOrder(overview.Name, overview.Id, contents);
            readingOrder.CoordinateTranslator = coordinateTranslator;

            return readingOrder;
        }
    }
}
