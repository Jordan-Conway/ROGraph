using ROGraph.Backend.DataProviders.Interfaces;
using ROGraph.Shared.Models;

namespace ROGraph.Backend.DataProviders.MockProviders;

public class MockReadingOrderListProvider : IReadingOrderListProvider
{
    public List<ReadingOrderOverview> GetReadingOrders()
    {
        var list = new List<ReadingOrderOverview>
        {
            new ReadingOrderOverview("Reading Order 1", Guid.NewGuid()),
            new ReadingOrderOverview("Reading Order 2", Guid.NewGuid()),
        };

        return list;
    }

    public ReadingOrderOverview? GetReadingOrderOverview(Guid id)
    {
        return new ReadingOrderOverview("Reading Order 1", Guid.NewGuid());
    }
}
