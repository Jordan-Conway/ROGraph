using ROGraph.Shared.Models;

namespace ROGraph.Backend.Data.DataProviders.Interfaces
{
    public interface IReadingOrderListProvider
    {
        public abstract List<ReadingOrderOverview> GetReadingOrders();

        public abstract ReadingOrderOverview? GetReadingOrderOverview(Guid id);
    }
}
