using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ROGraph.Backend.Context;
using ROGraph.Backend.DatabaseModels;
using ROGraph.Shared.Models;

namespace ROGraph.Backend.Repositories.ReadingOrders;

internal class ReadingOrderRepository : IReadingOrderRepository
{
    private readonly IDBContextFactory _dbContextFactory;

    public ReadingOrderRepository(IDBContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<IList<ReadingOrderOverview>> GetAllReadingOrders(CancellationToken token = default)
    {
        var context = _dbContextFactory.CreateDbContext();

        var overviews = context.GetSet<ReadingOrderOverviewDbModel>().Select(n => n.ToOverview());

        return await overviews.ToListAsync(token);
    }

    public async Task<ReadingOrderOverview?> GetReadingOrder(Guid id, CancellationToken token = default)
    {
        var context = _dbContextFactory.CreateDbContext();

        var overview = await context.GetSet<ReadingOrderOverviewDbModel>().FirstOrDefaultAsync(o => o.Id == id, token);

        return overview?.ToOverview();
    }

    public async Task<bool> CreateReadingOrder(ReadingOrderOverview readingOrderOverview, CancellationToken token = default)
    {
        if (readingOrderOverview.Id == Guid.Empty)
        {
            readingOrderOverview.Id = Guid.NewGuid();
        }

        var context = _dbContextFactory.CreateDbContext();

        var overview = readingOrderOverview.ToDbModel(ReadingOrderStatus.Active);

        await context.Add(overview, token);

        var rowsChanged = await context.Save(token);

        return rowsChanged > 0;
    }

    public async Task<bool> UpdateReadingOrder(ReadingOrderOverview readingOrder, CancellationToken token = default)
    {
        var context = _dbContextFactory.CreateDbContext();

        var existing = context.GetSet<ReadingOrderOverviewDbModel>().FirstOrDefault(o => o.Id == readingOrder.Id);

        if (existing is null)
        {
            return false;
        }

        existing.Name = readingOrder.Name;
        existing.Description = readingOrder.Description;
        existing.MaxX = readingOrder.MaxX;
        readingOrder.MaxY = readingOrder.MaxY;

        var rowsChanged = await context.Save(token);

        return rowsChanged == 1;
    }

    public async Task<bool> DeleteReadingOrder(Guid id, CancellationToken token = default)
    {
        var context = _dbContextFactory.CreateDbContext();

        var existing = await context.GetSet<ReadingOrderOverviewDbModel>().FirstOrDefaultAsync(o => o.Id == id, token);

        if (existing is null)
        {
            return false;
        }

        existing.Status = ReadingOrderStatus.Deleted;

        var rowsChanged = await context.Save(token);

        return rowsChanged == 1;
    }
}
