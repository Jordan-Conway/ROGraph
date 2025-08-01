using ROGraph.Data;
using ROGraph.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ROGraph.Pages
{
    /// <summary>
    /// Interaction logic for ReadingOrderListPage.xaml
    /// </summary>
    public partial class ReadingOrderListPage : Page
    {
        public ReadingOrderListPage()
        {
            InitializeComponent();
            this.PopulateList();
        }

        private void PopulateList()
        {
            Dictionary<int, ReadingOrder> roList = ReadingOrders.Items ?? [];

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
            var itemNum = ((Button)sender).Tag;
            System.Diagnostics.Debug.WriteLine("Moving to " + itemNum);
        }
    }
}
