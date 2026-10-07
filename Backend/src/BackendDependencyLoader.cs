using Microsoft.Extensions.DependencyInjection;
using ROGraph.Backend.Context;
using ROGraph.Backend.Contracts;
using ROGraph.Backend.DataProviders;
using ROGraph.Backend.Repositories.Connectors;
using ROGraph.Backend.Repositories.Nodes;
using ROGraph.Backend.Repositories.ReadingOrders;
using ROGraph.Backend.Services;

namespace ROGraph.Backend;

public static class BackendDependencyLoader
{
    public static void AddBackendDependencies(this IServiceCollection services)
    {
        services.AddSingleton<IDBContextFactory, DBContextFactory>();
        services.AddSingleton<IReadingOrderDataSourceCreator, SqlDataSourceCreator>();

        AddRepositories(services);
        AddServices(services);
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddSingleton<INodeRepository, NodeRepository>();
        services.AddSingleton<IConnectorRepository, ConnectorRepository>();
        services.AddSingleton<IReadingOrderRepository, ReadingOrderRepository>();
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddSingleton<IReadingOrderService, ReadingOrderService>();
    }
}
