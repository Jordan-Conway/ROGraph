using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ROGraph.Shared.Models;

namespace ROGraph.Backend.Contracts;

public interface IReadingOrderService
{
    public Task<IList<ReadingOrderOverview>> GetReadingOrderOverviews(CancellationToken token = default);
    public Task<ReadingOrderOverview?> GetReadingOrderOverview(Guid id, CancellationToken token = default);
    public Task<bool> UpdateReadingOrderOverview(ReadingOrderOverview readingOrderOverview, CancellationToken token = default);
    public Task<ReadingOrder?> GetReadingOrder(Guid id, CancellationToken token = default);
    public Task<bool> CreateReadingOrder(ReadingOrderOverview overview, CancellationToken token = default);
    public Task<bool> UpdateReadingOrder(ReadingOrder readingOrder, CancellationToken token = default);
    public Task<bool> DeleteReadingOrder(Guid id, CancellationToken token = default);
}