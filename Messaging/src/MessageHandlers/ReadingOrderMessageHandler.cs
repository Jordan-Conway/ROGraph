using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using ROGraph.Backend.Contracts;
using ROGraph.Messaging.Messages;

namespace ROGraph.Messaging.MessageHandlers;

internal class ReadingOrderMessageHandler
{
    private readonly IReadingOrderService _readingOrderService;

    public ReadingOrderMessageHandler(IReadingOrderService readingOrderService)
    {
        _readingOrderService = readingOrderService;

        RegisterMessageHandlers();
    }

    private void RegisterMessageHandlers()
    {
        WeakReferenceMessenger.Default.Register<GetReadingOrderRequest>(this , (_, m) =>
        {
            var result = _readingOrderService.GetReadingOrder(m.ReadingOrderId);
            
            m.Reply(result);
        });
        
        WeakReferenceMessenger.Default.Register<GetReadingOrderOverviewRequest>(this, (_, m) =>
        {
            var result = _readingOrderService.GetReadingOrderOverview(m.ReadingOrderId);
            
            m.Reply(result);
        });
        
        WeakReferenceMessenger.Default.Register<GetReadingOrderOverviewsRequest>(this, (_, m) =>
        {
            var result = _readingOrderService.GetReadingOrderOverviews();
            
            m.Reply(result);
        });
        
        WeakReferenceMessenger.Default.Register<AddReadingOrderRequest>(this, (_, m) =>
        {
            var result = _readingOrderService.CreateReadingOrder(m.Overview);

            m.Reply(result);
        });

        WeakReferenceMessenger.Default.Register<UpdateReadingOrderRequest>(this, (_, m) =>
        {
            var result = _readingOrderService.UpdateReadingOrderOverview(m.Overview);

            m.Reply(result);
        });

        WeakReferenceMessenger.Default.Register<UpdateReadingOrderContentRequest>(this, (_, m) =>
        {
            var result = _readingOrderService.UpdateReadingOrder(m.ReadingOrder);

            m.Reply(result);
        });

        WeakReferenceMessenger.Default.Register<DeleteReadingOrderRequest>(this, (_, m) =>
        {
            var result = _readingOrderService.DeleteReadingOrder(m.ReadingOrderId);

            m.Reply(result);
        });
    }
}
