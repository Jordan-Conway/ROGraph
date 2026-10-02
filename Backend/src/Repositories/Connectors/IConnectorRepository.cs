using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ROGraph.Backend.DatabaseModels;
using ROGraph.Shared.Models;

namespace ROGraph.Backend.Repositories.Connectors;

internal interface IConnectorRepository
{
    Task<IList<Connector>> GetConnectorsForReadingOrder(Guid readingOrderId, CoordinateTranslator coordinateTranslator, CancellationToken token = default);
    Task CreateConnector(Connector connector, CoordinateTranslator coordinateTranslator, CancellationToken token = default);
}