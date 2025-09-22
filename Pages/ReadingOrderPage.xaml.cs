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

        private const int IMAGE_SIZE = 240;
        private const int IMAGE_GAP_SIZE = 80;
        public ReadingOrderPage(ReadingOrder readingOrder)
        {
            this.readingOrder = readingOrder;
            InitializeComponent();
            PlaceNodes();
        }

        private void PlaceNodes()
        {
            if(readingOrder.Nodes.Count() == 0)
            {
                Console.WriteLine("No Nodes to Place");
                return;
            }

            // Place in the connectors first
            foreach(List<Line> list in this.readingOrder.Nodes.Connectors)
            {
                foreach (Line line in list)
                {
                    this.PlaceLine(line);
                }
            }

            // Fill in the nodes
            int columnNumber = 0;
            foreach (List<Node?> list in this.readingOrder.Nodes.Nodes)
            {
                int rowNumber = 0;
                foreach(Node? node in list)
                {
                    if (node != null)
                    {
                        this.PlaceNode(node, rowNumber, columnNumber);
                    }
                    rowNumber++;
                }
                columnNumber++;
            }

            return;
        }

        private void PlaceLine(Line line)
        {
            line.Stroke = System.Windows.Media.Brushes.Black;
            line.SnapsToDevicePixels = true;
            line.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
            line.StrokeThickness = 10;
            ReadingOrderCanvas.Children.Add(line);
            System.Diagnostics.Debug.WriteLine("Placed a line");
        }

        private void PlaceNode(Node node, int rowNumber, int columnNumber)
        {
            UIElement display = (UIElement)((DataTemplate)this.Resources["NodeDisplay"]).LoadContent();
            ((FrameworkElement)display).DataContext = node;

            int x = node.X + (columnNumber * IMAGE_SIZE) + (columnNumber * IMAGE_GAP_SIZE);
            int y = node.Y + (rowNumber * IMAGE_SIZE) + (rowNumber * (IMAGE_GAP_SIZE/2));

            Canvas.SetLeft(display, x);
            Canvas.SetTop(display, y);

            ReadingOrderCanvas.Children.Add(display);
            System.Diagnostics.Debug.WriteLine("Place a node");
        }
    }
}
