using Microsoft.Extensions.DependencyInjection;
using ROGraph.Backend.Context;
using ROGraph.Backend.Contracts;
using ROGraph.Backend.DataProviders.SQLiteProviders;
using ROGraph.Backend.Repositories.Nodes;

namespace ROGraph.Backend;

public static class BackendDependencyLoader
{
    public static void AddDependencies (IServiceCollection services)
    {
        services.AddTransient<IDBContextFactory, DBContextFactory>();
        services.AddSingleton<IReadingOrderDataSourceCreator, SqlDataSourceCreator>();
        services.AddSingleton<IReadingOrderProvider, ReadingOrderListProvider>();
        services.AddSingleton<INodeRepository, NodeRepository>();
    }
}