using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using ROGraph.Backend.Contracts;
using ROGraph.Shared.Models;

namespace ROGraph.Backend.Services;

public class ReadingOrderService : IReadingOrderService
{
    private readonly IReadingOrderProvider _readingOrderProvider;
    
    public ReadingOrderService(IReadingOrderProvider readingOrderProvider)
    {
        _readingOrderProvider = readingOrderProvider;
    }
    
    public async Task<IList<ReadingOrderOverview>> GetReadingOrderOverviews(CancellationToken token = default)
    {
        return await _readingOrderProvider.GetReadingOrders(token);
    }

    public async Task<ReadingOrderOverview?> GetReadingOrderOverview(Guid id, CancellationToken token = default)
    {
        return await _readingOrderProvider.GetReadingOrderOverview(id, token);
    }

    public Task<bool> UpdateReadingOrderOverview(ReadingOrderOverview readingOrderOverview, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public async Task<ReadingOrder?> GetReadingOrder(Guid id, CancellationToken token = default)
    {
        return await _readingOrderProvider.GetReadingOrder(id, token);
    }

    public async Task<bool> CreateReadingOrder(ReadingOrderOverview overview, CancellationToken token = default)
    {
        return await _readingOrderProvider.CreateReadingOrder(overview, token);
    }

    public Task<bool> UpdateReadingOrder(ReadingOrder readingOrder, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteReadingOrder(Guid id, CancellationToken token = default)
    {
        if (id == Guid.Empty)
        {
            return false;
        }

        return await _readingOrderProvider.DeleteReadingOrder(id, token);
    }
}