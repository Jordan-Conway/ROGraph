using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ROGraph.Shared.Models;

namespace ROGraph.Backend.Repositories.ReadingOrders;

internal interface IReadingOrderRepository
{
    Task<IList<ReadingOrderOverview>> GetAllReadingOrders(CancellationToken token = default);

    Task<ReadingOrderOverview?> GetReadingOrder(Guid id, CancellationToken token = default);

    Task<bool> CreateReadingOrder(ReadingOrderOverview readingOrderOverview, CancellationToken token = default);

    Task<bool> UpdateReadingOrder(ReadingOrderOverview readingOrderOverview, CancellationToken token = default);

    Task<bool> DeleteReadingOrder(Guid id, CancellationToken token = default);
}
