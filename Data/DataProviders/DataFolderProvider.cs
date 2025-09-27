using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROGraph.Data.DataProviders
{
    public class DataFolderProvider
    {
        private static string _dataDirectory = "";

        private static readonly string CREATE_NODES_TABLE = 
            "create table nodes(id Text PRIMARY KEY, name Text, origin Text, " +
            "created Integer, lastModified Integer, isCompleted Integer, " +
            "checklist Blob, description Text);";

        private static readonly string ADD_NODES_TRIGGERS =
            "Create Trigger updated_last_modified after update of id, name, origin, created, isCompleted, checklist, description on nodes " +
            "For Each Row When OLD.lastModified = NEW.lastModified " +
            "Begin Update nodes Set lastModified = datetime('now') Where id = NEW.id End;";

        public static string GetDataDirectory()
        {
            if (_dataDirectory != String.Empty)
            {
                return _dataDirectory;
            }

            DirectoryInfo currentDir = new DirectoryInfo(Environment.CurrentDirectory);

            while (currentDir.GetDirectories("Data").Length == 0)
            {
                if (currentDir.Parent == null)
                {
                    throw new NullReferenceException("Reached null parent while looking for Data folder");
                }
                currentDir = currentDir.Parent;
            }

            _dataDirectory = currentDir.GetDirectories("Data")[0].FullName;

            return _dataDirectory;
        }

        public static void SetupDataDirectory()
        {
            if (_dataDirectory == String.Empty)
            {
                GetDataDirectory();
            }

            string saveDataPath = _dataDirectory + "SaveData/";

            if (!Directory.Exists(saveDataPath))
            {
                Directory.CreateDirectory(saveDataPath);
            }

            CreateNodesDB(saveDataPath);
            CreateConnectorsDB(saveDataPath);
        }

        private static bool CreateNodesDB(string path)
        {
            if(File.Exists(path + "Nodes.db"))
            {
                return false;
            }

            throw new NotImplementedException("Creating Nodes.db not implemented");
        }

        private static bool CreateConnectorsDB(string path)
        {
            if (File.Exists(path + "Nodes.db"))
            {
                return false;
            }

            throw new NotImplementedException("Creating Connectors.db not implemented");
        }
    }
}
