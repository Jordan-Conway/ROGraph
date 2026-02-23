using CommunityToolkit.Mvvm.Messaging;
using ROGraph.Shared.Models;
using ROGraph.UI.Messages;

namespace ROGraph.UI.Dispatchers;

internal static class ReadingOrderListViewDispatcher
{
    public static void DispatchReadingOrderAddedMessage(ReadingOrderOverview overview)
    {
        var message = new ReadingOrderAddedMessage(overview);
        WeakReferenceMessenger.Default.Send(message);
    }
}