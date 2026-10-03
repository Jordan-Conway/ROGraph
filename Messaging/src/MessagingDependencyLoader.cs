using System;
using Microsoft.Extensions.DependencyInjection;
using ROGraph.Messaging.MessageHandlers;

namespace ROGraph.Messaging;

public static class MessagingDependencyLoader
{
    public static void SetupMessaging(this IServiceProvider provider)
    {
        _ = provider.GetRequiredService<MessageHandler>();
    }

    public static void AddMessagingDependencies(this IServiceCollection services)
    {
        services.AddSingleton<MessageHandler>();
        services.AddSingleton<ReadingOrderMessageHandler>();
    }
}
