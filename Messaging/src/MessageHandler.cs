using ROGraph.Messaging.MessageHandlers;

namespace ROGraph.Messaging;

///Wrapper class to ensure all message handlers are instansiated
internal class MessageHandler
{
    public MessageHandler(ReadingOrderMessageHandler readingOrderMessageHandler)
    {
        _ = readingOrderMessageHandler;
    }
}
