using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ROGraph.Backend;
using ROGraph.Messaging;
using ROGraph.UI.Controls;
using ROGraph.UI.Services;
using ROGraph.UI.Services.StateService;
using ROGraph.UI.Services.ThemeService;
using ROGraph.UI.Views;
using ROGraph.UI.Views.ReadingOrderView;
using ReadingOrderListViewModel = ROGraph.UI.ViewModels.ReadingOrderListViewModel;

namespace ROGraph.UI;

internal static class DependencyLoader
{
    extension(IServiceCollection services)
    {
        public void RegisterDependencies()
        {
            services.AddSingleton<IDialogService, DialogService>();
            services.AddSingleton<IMessagingService, MessagingService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<IStateService, StateService>();
            services.AddSingleton<IThemeManager, ThemeService>();
            services.AddSingleton<IThemeToggler, ThemeService>();
        
            services.AddSingleton<ILoggerFactory, LoggerFactory>();

            services.AddKeyedSingleton<IDialogContainer, DialogContainer>("dialogContainer");
        
            services.AddBackendDependencies();
            services.AddMessagingDependencies();
            services.RegisterPages();
        }

        private void RegisterPages()
        {
            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<MainWindow>();

            services.AddTransient<ReadingOrderListViewModel>();
            services.AddTransient<ReadingOrderListView>();

            services.AddTransient<ReadingOrderViewModel>();
            services.AddTransient<ReadingOrderView>();
        }
    }
}