using CommunityToolkit.Mvvm.Messaging;
using ROGraph.Backend.Contracts;
using ROGraph.Messaging.Messages;

namespace ROGraph.Messaging.MessageHandlers;

internal class ReadingOrderMessageHandler
{
    private readonly IMessenger _messenger;
    private readonly IReadingOrderService _readingOrderService;

    public ReadingOrderMessageHandler(IMessenger messenger, IReadingOrderService readingOrderService)
    {
        _messenger  = messenger;
        _readingOrderService = readingOrderService;

        RegisterMessageHandlers();
    }

    private void RegisterMessageHandlers()
    {
        _messenger.Register<GetReadingOrderRequest>(this , (_, m) =>
        {
            var result = _readingOrderService.GetReadingOrder(m.ReadingOrderId);
            
            m.Reply(result);
        });

        _messenger.Register<GetReadingOrderOverviewRequest>(this, (_, m) =>
        {
            var result = _readingOrderService.GetReadingOrderOverview(m.ReadingOrderId);
            
            m.Reply(result);
        });
        
        _messenger.Register<GetReadingOrderOverviewsRequest>(this, (_, m) =>
        {
            var result = _readingOrderService.GetReadingOrderOverviews();
            
            m.Reply(result);
        });
        
        _messenger.Register<AddReadingOrderRequest>(this, (_, m) =>
        {
            var result = _readingOrderService.CreateReadingOrder(m.Overview);

            m.Reply(result);
        });

        _messenger.Register<UpdateReadingOrderRequest>(this, (_, m) =>
        {
            var result = _readingOrderService.UpdateReadingOrderOverview(m.Overview);

            m.Reply(result);
        });

        _messenger.Register<UpdateReadingOrderContentRequest>(this, (_, m) =>
        {
            var result = _readingOrderService.UpdateReadingOrder(m.ReadingOrder);

            m.Reply(result);
        });

        _messenger.Register<DeleteReadingOrderRequest>(this, (_, m) =>
        {
            var result = _readingOrderService.DeleteReadingOrder(m.ReadingOrderId);

            m.Reply(result);
        });
    }
}
