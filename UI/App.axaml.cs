using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ROGraph.Backend;
using ROGraph.Backend.DataProviders.Interfaces;
using ROGraph.UI.ViewModels;
using ROGraph.UI;

namespace UI;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
       DisableAvaloniaDataAnnotationValidation();

       var collection = new ServiceCollection();
       BackendDependencyLoader.AddDependencies(collection);
       AddDependencies(collection);
       
       var services = collection.BuildServiceProvider();

       var dataSourceCreator = services.GetRequiredService<IReadingOrderDataSourceCreator>();
       dataSourceCreator.CreateDataSource();
       
       if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
       {
           desktop.MainWindow = services.GetRequiredService<MainWindow>();
       }

       base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }

    private void AddDependencies(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<ILoggerFactory, LoggerFactory>();
        serviceCollection.AddSingleton<MainWindow, MainWindow>();
    }
}