using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using ROGraph.Messaging.Messages;
using ROGraph.Shared.Models;

namespace ROGraph.UI.Services;

internal class MessagingService : IMessagingService
{
    private readonly IMessenger _messenger;

    public MessagingService(IMessenger messenger)
    {
        _messenger = messenger;
    }
    
    public async Task<IList<ReadingOrderOverview>> GetReadingOrderOverviews(CancellationToken token = default)
    {
        var message = new GetReadingOrderOverviewsRequest();
        return await _messenger.Send(message);
    }
}