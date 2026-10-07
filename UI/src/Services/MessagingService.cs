using System;
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

    public async Task<ReadingOrder?> GetReadingOrder(Guid readingOrderId, CancellationToken token = default)
    {
        var message = new GetReadingOrderRequest(readingOrderId);
        return await _messenger.Send(message);
    }

    public async Task<bool> SaveReadingOrder(ReadingOrder readingOrder, CancellationToken token = default)
    {
        var message = new UpdateReadingOrderContentRequest(readingOrder);
        return await _messenger.Send(message);
    }

    public async Task<bool> CreateReadingOrder(ReadingOrderOverview readingOrderOverview,
        CancellationToken token = default)
    {
        var message = new AddReadingOrderRequest(readingOrderOverview);
        return await _messenger.Send(message);
    }

    public async Task<bool> UpdateReadingOrder(ReadingOrderOverview readingOrderOverview,
        CancellationToken token = default)
    {
        var message = new UpdateReadingOrderRequest(readingOrderOverview);
        return await _messenger.Send(message);
    }

    public async Task<bool> DeleteReadingOrder(Guid readingOrderId, CancellationToken token = default)
    {
        var message = new DeleteReadingOrderRequest(readingOrderId);
        return await _messenger.Send(message);
    }
}