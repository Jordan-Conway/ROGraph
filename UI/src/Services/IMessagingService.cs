using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ROGraph.Shared.Models;

namespace ROGraph.UI.Services;

public interface IMessagingService
{
    public Task<IList<ReadingOrderOverview>> GetReadingOrderOverviews(CancellationToken token = default);

    public Task<ReadingOrder?> GetReadingOrder(Guid readingOrderId, CancellationToken token = default);
    
    public Task<bool> SaveReadingOrder(ReadingOrder readingOrder, CancellationToken token = default);
    
    public Task<bool> CreateReadingOrder(ReadingOrderOverview readingOrder, CancellationToken token = default);

    public Task<bool> UpdateReadingOrder(ReadingOrderOverview readingOrder, CancellationToken token = default);

    public Task<bool> DeleteReadingOrder(Guid readingOrderId, CancellationToken token = default);
}