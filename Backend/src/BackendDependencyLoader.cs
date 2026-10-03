using Microsoft.Extensions.DependencyInjection;
using ROGraph.Backend.Context;
using ROGraph.Backend.Contracts;
using ROGraph.Backend.DataProviders.SQLiteProviders;
using ROGraph.Backend.Repositories.Connectors;
using ROGraph.Backend.Repositories.Nodes;
using ROGraph.Backend.Repositories.ReadingOrders;
using ROGraph.Backend.Services;

namespace ROGraph.Backend;

public static class BackendDependencyLoader
{
    public static void AddDependencies(IServiceCollection services)
    {
        services.AddTransient<IDBContextFactory, DBContextFactory>();
        services.AddSingleton<IReadingOrderDataSourceCreator, SqlDataSourceCreator>();
        services.AddSingleton<IReadingOrderProvider, ReadingOrderListProvider>();

        AddRepositories(services);
        AddServices(services);
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddTransient<INodeRepository, NodeRepository>();
        services.AddTransient<IConnectorRepository, ConnectorRepository>();
        services.AddTransient<IReadingOrderRepository, ReadingOrderRepository>();
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddTransient<IReadingOrderService, ReadingOrderService>();
    }
}
