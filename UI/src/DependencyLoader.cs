using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ROGraph.Backend;
using ROGraph.Messaging;
using ROGraph.UI.Services;
using ROGraph.UI.Services.StateService;
using ROGraph.UI.Views.ReadingOrderListView;
using ROGraph.UI.Views.ReadingOrderView;

namespace ROGraph.UI;

internal static class DependencyLoader
{
    extension(IServiceCollection services)
    {
        public void RegisterDependencies()
        {
            services.AddSingleton<IMessagingService, MessagingService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<IStateService, StateService>();
        
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
        
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