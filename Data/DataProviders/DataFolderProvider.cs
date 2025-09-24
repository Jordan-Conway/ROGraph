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

            return currentDir.GetDirectories("Data")[0].FullName;
        }
    }
}
