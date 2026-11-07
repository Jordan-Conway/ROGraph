using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ROGraph.Backend;
using ROGraph.Backend.Contracts;
using ROGraph.Messaging;
using ROGraph.UI.Services.ThemeService;

namespace ROGraph.UI;

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
       collection.RegisterDependencies();
       
       var services = collection.BuildServiceProvider();
       services.SetupMessaging();

       var dataSourceCreator = services.GetRequiredService<IReadingOrderDataSourceCreator>();
       dataSourceCreator.CreateDataSource();
       
       var themeService = services.GetRequiredService<IThemeService>();
       RegisterThemes(themeService);
       
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

    private void RegisterThemes(IThemeService themeService)
    {
        var themes = themeService.GetAllThemeKeys();
        foreach (var themeKey in themes)
        {
            var theme = themeService.GetTheme(themeKey);
            var resourceDictionary = ThemeConvertor.ConvertTheme(theme);

            var themeVariant = new ThemeVariant(themeKey, ThemeVariant.Dark);
            Resources.ThemeDictionaries.Add(themeVariant, resourceDictionary);
        }

        RequestedThemeVariant = themeService.GetSelectedTheme();
    }
}