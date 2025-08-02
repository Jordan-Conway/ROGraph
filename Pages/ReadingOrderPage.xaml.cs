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
    /// Interaction logic for ReadingOrderPage.xaml
    /// </summary>
    public partial class ReadingOrderPage : Page
    {
        private ReadingOrder readingOrder;
        public ReadingOrderPage(ReadingOrder readingOrder)
        {
            this.readingOrder = readingOrder;
            InitializeComponent();
            PopulateNodes();
        }

        private void PopulateNodes()
        {
            Node newNode = new Node { Name = "My Reading Order", Type = Enums.NodeType.Diamond };
            UIElement display = (UIElement)((DataTemplate)this.Resources["NodeDisplay"]).LoadContent();
            ((FrameworkElement)display).DataContext = newNode;

            Canvas.SetTop(display, 100);
            Canvas.SetLeft(display, 20);

            ReadingOrderCanvas.Children.Add(display);
        }
    }
}
