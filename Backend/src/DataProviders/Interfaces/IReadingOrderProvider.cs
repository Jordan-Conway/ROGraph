using ROGraph.Shared.Models;

namespace ROGraph.Backend.DataProviders.Interfaces;

public interface IReadingOrderProvider
{
    public List<ReadingOrderOverview> GetReadingOrders();
    
    public ReadingOrder? GetReadingOrder(Guid id);

    public ReadingOrderOverview? GetReadingOrderOverview(Guid id);
    
    public bool CreateReadingOrder(ReadingOrderOverview overview);
    
    public bool UpdateReadingOrder(ReadingOrder readingOrder);
    
    public bool DeleteReadingOrder(Guid id);
}
