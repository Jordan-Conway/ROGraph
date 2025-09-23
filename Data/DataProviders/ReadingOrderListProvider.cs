using Newtonsoft.Json;
using ROGraph.Models;
using System.IO;

namespace ROGraph.Data.DataProviders
{
    public class ReadingOrderListProvider
    {
        public static List<ReadingOrderOverview> GetReadingOrders()
        {
            DirectoryInfo currentDir = new DirectoryInfo(Environment.CurrentDirectory);

            while(currentDir.GetDirectories("Data").Length == 0)
            {
                if(currentDir.Parent == null)
                {
                    throw new NullReferenceException("Reached null parent while looking for Data folder");
                }
                currentDir = currentDir.Parent;
            }

            DirectoryInfo directoryInfo = new DirectoryInfo(currentDir + @"\Data\ReadingOrders\");
            List<ReadingOrderOverview> overviews = new List<ReadingOrderOverview>();

            foreach (DirectoryInfo readingOrder in directoryInfo.EnumerateDirectories())
            {
                FileInfo file = readingOrder.GetFiles("info.json")[0];
                
                ReadingOrderOverview? overview = LoadInfoFromFile(file);

                if (overview == null)
                {
                    continue;
                }

                overviews.Add(overview);

            }
            return overviews;
        }

        private static ReadingOrderOverview? LoadInfoFromFile(FileInfo file)
        {
            using (StreamReader sr = new StreamReader(file.FullName))
            {
                string info = sr.ReadToEnd();
                return JsonConvert.DeserializeObject<ReadingOrderOverview>(info);
            }
        }
    }
}
