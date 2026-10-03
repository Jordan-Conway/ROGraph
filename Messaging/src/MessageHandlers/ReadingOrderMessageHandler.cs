using CommunityToolkit.Mvvm.Messaging;
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
        WeakReferenceMessenger.Default.Register<AddReadingOrderMessage>(this, (_, m) =>
        {
            var result = _readingOrderService.CreateReadingOrder(m.Overview);

            m.Reply(result);
        });

        WeakReferenceMessenger.Default.Register<UpdateReadingOrderMessage>(this, (_, m) =>
        {
            var result = _readingOrderService.UpdateReadingOrderOverview(m.Overview);

            m.Reply(result);
        });

        WeakReferenceMessenger.Default.Register<UpdateReadingOrderContentMessage>(this, (_, m) =>
        {
            var result = _readingOrderService.UpdateReadingOrder(m.ReadingOrder);

            m.Reply(result);
        });

        WeakReferenceMessenger.Default.Register<DeleteReadingOrderMessage>(this, (_, m) =>
        {
            var result = _readingOrderService.DeleteReadingOrder(m.ReadingOrderId);

            m.Reply(result);
        });
    }
}
