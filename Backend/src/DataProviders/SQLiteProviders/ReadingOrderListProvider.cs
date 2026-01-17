using System.Data.SQLite;
using ROGraph.Backend.DataProviders.Interfaces;
using ROGraph.Shared.Models;

namespace ROGraph.Backend.DataProviders.SQLiteProviders;

public class ReadingOrderListProvider : IReadingOrderListProvider
{
    private static readonly string ConnectionString = "Data Source = " + FilePathProvider.GetDatabaseFilePath(); 

    public ReadingOrderOverview? GetReadingOrderOverview(Guid id)
    {
        throw new NotImplementedException();
    }

    public List<ReadingOrderOverview> GetReadingOrders()
    {
        throw new NotImplementedException();
    }
}