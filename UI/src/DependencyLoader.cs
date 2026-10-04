using Microsoft.Extensions.DependencyInjection;
using ROGraph.Backend;
using ROGraph.Messaging;
using ROGraph.UI.Services;

namespace ROGraph.UI;

internal static class DependencyLoader
{
    public static void RegisterServices(IServiceCollection services)
    {
        services.AddSingleton<IMessagingService, MessagingService>();
        
        services.AddBackendDependencies();
        services.AddMessagingDependencies();
    }
}