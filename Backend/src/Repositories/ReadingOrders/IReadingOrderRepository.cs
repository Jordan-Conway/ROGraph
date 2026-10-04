using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ROGraph.Shared.Models;

namespace ROGraph.Backend.Repositories.ReadingOrders;

internal interface IReadingOrderRepository
{
    Task<IList<ReadingOrderOverview>> GetAllReadingOrders(CancellationToken token = default);

    public Task<ReadingOrderOverview?> GetReadingOrder(Guid id, CancellationToken token = default);

    public Task<bool> CreateReadingOrder(ReadingOrderOverview readingOrderOverview, CancellationToken token = default);

    public Task<bool> UpdateReadingOrder(ReadingOrderOverview readingOrderOverview, CancellationToken token = default);

    public Task<bool> DeleteReadingOrder(Guid id, CancellationToken token = default);
}
