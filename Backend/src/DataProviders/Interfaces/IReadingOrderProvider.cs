using ROGraph.Shared.Models;

namespace ROGraph.Backend.DataProviders.Interfaces;

public interface IReadingOrderProvider
{
    public abstract List<ReadingOrderOverview> GetReadingOrders();
    
    public abstract ReadingOrder? GetReadingOrder(Guid id);

    public abstract ReadingOrderOverview? GetReadingOrderOverview(Guid id);
    
    public abstract bool CreateReadingOrder(string name, string? description);
    
    public abstract bool UpdateReadingOrder(ReadingOrder readingOrder);
}
