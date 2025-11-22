using Avalonia.Controls;
using ROGraph.Backend.Data.DataProviders.Interfaces;
using ROGraph.Backend.Data.DataProviders.MockProviders;
using ROGraph.Backend.Data.DataProviders.SQLiteProviders;
using ROGraph.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ROGraph.Pages
{
    /// <summary>
    /// Interaction logic for ReadingOrderListPage.xaml
    /// </summary>
    public partial class ReadingOrderListPage : UserControl
    {
        public ReadingOrderListPage()
        {
            InitializeComponent();
            this.PopulateList();
        }

        private void PopulateList()
        {
            IReadingOrderListProvider readingOrderListProvider;

            if (Environment.GetEnvironmentVariable("USE_MOCK_PROVIDERS") == "true")
            {
                readingOrderListProvider = new MockReadingOrderListProvider();
            }
            else
            {
                readingOrderListProvider = new ReadingOrderListProvider();
            }

            List<ReadingOrderOverview> roList = readingOrderListProvider.GetReadingOrders();

            for (int i = 0; i < roList.Count; i++)
            {
                var template = (DataTemplate)this.Resources["ReadingOrder"];
                var listItem = template.LoadContent();
                ((FrameworkElement)listItem).DataContext = roList[i];

                ReadingOrderList.RowDefinitions.Add(new RowDefinition());

                int rowNum = ReadingOrderList.RowDefinitions.Count - 1;
                Grid.SetRow((UIElement)listItem, rowNum);

                System.Diagnostics.Debug.WriteLine("Adding Child on Row: " + rowNum);

                ReadingOrderList.Children.Add((UIElement)listItem);
            }
        }

        private void NavigateToPage(object sender, RoutedEventArgs e)
        {
            Guid id = (Guid)((Button)sender).Tag;
            System.Diagnostics.Debug.WriteLine("Moving to Reading Order " + id);
            NavigationService navigationService = NavigationService.GetNavigationService(this);

            ReadingOrderPage roPage;
            try
            {
                roPage = new ReadingOrderPage(id);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception when accessing " + id + " in ReadingOrderList");
                //TODO: Show error to user
                return;
            }

            navigationService.Navigate(roPage);
             
        }
    }
}
