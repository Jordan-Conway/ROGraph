using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ROGraph.Shared.Models;

namespace ROGraph.Backend.DataProviders.Interfaces;

public interface IReadingOrderProvider
{
    public List<ReadingOrderOverview> GetReadingOrders();
    
    public ReadingOrder? GetReadingOrder(Guid id);

    public ReadingOrderOverview? GetReadingOrderOverview(Guid id);
    
    public bool UpdateReadingOrderOverview(ReadingOrderOverview readingOrderOverview);
    
    public bool CreateReadingOrder(ReadingOrderOverview overview);
    
    public Task<bool> UpdateReadingOrder(ReadingOrder readingOrder, CancellationToken token = default);
    
    public bool DeleteReadingOrder(Guid id);
}
