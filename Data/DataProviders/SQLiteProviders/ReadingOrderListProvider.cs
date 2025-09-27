using Newtonsoft.Json;
using ROGraph.Data.DataProviders.Interfaces;
using ROGraph.Models;
using System.IO;

namespace ROGraph.Data.DataProviders.SQLiteProviders
{
    public class ReadingOrderListProvider: IReadingOrderListProvider
    {
        public List<ReadingOrderOverview> GetReadingOrders()
        {
            DirectoryInfo directoryInfo = GetSaveDataDirectory();
            
            List<ReadingOrderOverview> overviews = new List<ReadingOrderOverview>();

            foreach (FileInfo f in directoryInfo.EnumerateFiles("*.json"))
            {
                ReadingOrderOverview? overview = LoadInfoFromFile(f);

                if (overview == null)
                {
                    continue;
                }

                overviews.Add(overview);

            }
            return overviews;
        }

        public ReadingOrderOverview? GetReadingOrderOverview(Guid id)
        {
            DirectoryInfo directory = GetSaveDataDirectory();
            FileInfo f = directory.GetFiles(id.ToString() + ".json")[0];
            return LoadInfoFromFile(f);
        }

        private static ReadingOrderOverview? LoadInfoFromFile(FileInfo file)
        {
            using (StreamReader sr = new StreamReader(file.FullName))
            {
                string info = sr.ReadToEnd();
                return JsonConvert.DeserializeObject<ReadingOrderOverview>(info);
            }
        }

        private static DirectoryInfo GetSaveDataDirectory()
        {
            string currentDir = DataFolderProvider.GetDataDirectory();
            return new DirectoryInfo(currentDir + @"\SaveData\");
        }
    }
}
