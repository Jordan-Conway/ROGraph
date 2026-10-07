using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ROGraph.Backend.Context;
using ROGraph.Backend.DatabaseModels;
using ROGraph.Shared.Models;

namespace ROGraph.Backend.Repositories.Connectors;

internal class ConnectorRepository : IConnectorRepository
{
    private readonly IDBContextFactory _dbContextFactory;

    public ConnectorRepository(IDBContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<IList<Connector>> GetConnectorsForReadingOrder(Guid readingOrderId, CoordinateTranslator translator, CancellationToken token)
    {
        var context = _dbContextFactory.CreateDbContext();

        var dbConnectors = context.GetSet<ConnectorDbModel>().Where(c => c.ReadingOrderId == readingOrderId);

        return await dbConnectors.Select(c => translator.ToConnector(c)).ToListAsync(token);
    }

    public async Task CreateConnector(Connector connector, Guid readingOrderId, CoordinateTranslator coordinateTranslator,
        CancellationToken token)
    {
        if (connector.Id == Guid.Empty)
        {
            connector.Id = Guid.NewGuid();
        }

        var context = _dbContextFactory.CreateDbContext();

        var dbConnector = coordinateTranslator.ToConnectorDbModel(connector, readingOrderId);

        await context.Add(dbConnector, token);
        await context.Save(token);
    }

    public async Task UpdateConnector(Connector connector, CoordinateTranslator coordinateTranslator,
        CancellationToken token)
    {
        if (connector.Id == Guid.Empty)
        {
            return;
        }

        var context = _dbContextFactory.CreateDbContext();

        var existing = context.GetSet<ConnectorDbModel>().FirstOrDefault(c => c.Id == connector.Id);

        if (existing is null)
        {
            return;
        }

        var origin = coordinateTranslator.Translate(connector.Origin);
        var destination = coordinateTranslator.Translate(connector.Destination);

        existing.X1 = origin.Item1;
        existing.Y1 = origin.Item2;
        existing.X2 = destination.Item1;
        existing.Y2 = destination.Item2;

        await context.Save(token);
    }

    public async Task DeleteConnectors(IEnumerable<Guid> connectorId, CancellationToken token)
    {
        var context = _dbContextFactory.CreateDbContext();
        
        var toDelete = context.GetSet<ConnectorDbModel>().Where(c => connectorId.Contains(c.Id));
        
        context.GetSet<ConnectorDbModel>().RemoveRange(toDelete);

        await context.Save(token);
    }
}
