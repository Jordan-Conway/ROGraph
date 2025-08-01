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
        public List<string> readingOrders;
        public ReadingOrderListPage()
        {
            InitializeComponent();
            this.readingOrders = ["My Reading Order 1", "My Reading Order 2"];
            this.PopulateList();
        }

        private void PopulateList()
        {
            foreach (string item in this.readingOrders)
            {
                ReadingOrderOverview newOv = new()
                {
                    Name = item,
                    Page = ""
                };

                var template = (DataTemplate)this.Resources["ReadingOrder"];
                var listItem = template.LoadContent();
                ((FrameworkElement)listItem).DataContext = newOv;

                ReadingOrderList.RowDefinitions.Add(new RowDefinition());

                int rowNum = ReadingOrderList.RowDefinitions.Count - 1;
                Grid.SetRow((UIElement)listItem, rowNum);

                ReadingOrderList.Children.Add((UIElement)listItem);
            }

            Console.WriteLine(ReadingOrderList.Children);
        }
    }
}
