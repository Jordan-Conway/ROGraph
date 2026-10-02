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

    public async Task CreateConnector(Connector connector, CoordinateTranslator coordinateTranslator,
        CancellationToken token)
    {
        if (connector.Id == Guid.Empty)
        {
            connector.Id = Guid.NewGuid();
        }
        
        var context = _dbContextFactory.CreateDbContext();

        var dbConnector = coordinateTranslator.ToConnectorDbModel(connector);

        await context.Add(dbConnector, token);
        await context.Save(token);
    }
}