using ROGraph.Shared.Models;

namespace ROGraph.Backend.DataProviders.Interfaces;

public interface IReadingOrderProvider
{
    public List<ReadingOrderOverview> GetReadingOrders();
    
    public ReadingOrder? GetReadingOrder(Guid id);

    public ReadingOrderOverview? GetReadingOrderOverview(Guid id);
    
    public bool CreateReadingOrder(string name, string? description);
    
    public bool UpdateReadingOrder(ReadingOrder readingOrder);
}
