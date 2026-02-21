using Microsoft.Extensions.DependencyInjection;
using ROGraph.Backend.DataProviders.Interfaces;
using ROGraph.Backend.DataProviders.SQLiteProviders;

namespace ROGraph.Backend;

public static class BackendDependencyLoader
{
    public static void AddDependencies (IServiceCollection services)
    {
        services.AddSingleton<IReadingOrderDataSourceCreator, SqlDataSourceCreator>();
        services.AddSingleton<IReadingOrderProvider, ReadingOrderListProvider>();
    }
}