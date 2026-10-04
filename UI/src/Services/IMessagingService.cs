using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ROGraph.Shared.Models;

namespace ROGraph.UI.Services;

internal interface IMessagingService
{
    public Task<IList<ReadingOrderOverview>> GetReadingOrderOverviews(CancellationToken token = default);
}