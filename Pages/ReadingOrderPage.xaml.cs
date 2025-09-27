using ROGraph.Data;
using ROGraph.Data.DataProviders.SQLiteProviders;
using ROGraph.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private Guid readingOrderId;
        private ReadingOrder? readingOrder;

        private const int IMAGE_SIZE = 240;
        private const int IMAGE_GAP_SIZE = 80;
        public ReadingOrderPage(Guid readingOrderId)
        {
            Loaded += OnPageLoaded;
            this.readingOrderId = readingOrderId;
            InitializeComponent();
        }

        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            this.GetReadingOrder(this.readingOrderId);
            if (this.readingOrder != null)
            {
                Debug.WriteLine("Reading order is " + this.readingOrder.Name);
                PlaceNodes();
            }
        }

        private void PlaceNodes()
        {
            if(this.readingOrder == null)
            {
                Debug.WriteLine("Reading Order is Null");
                return;
            }

            if(this.readingOrder.Contents == null)
            {
                Debug.WriteLine("Reading Order has no content");
                return;
            }

            if(this.readingOrder.CoordinateTranslator == null)
            {
                throw new NullReferenceException("Reading Order has no translator");
            }

            Debug.WriteLine("Connectors is Empty: " + (this.readingOrder.Contents.Connectors.Count == 0));
            Debug.WriteLine("Nodes is Empty: " + (this.readingOrder.Contents.Nodes.Count == 0));

            // Place in the connectors first
            foreach (Connector connector in this.readingOrder.Contents.Connectors)
            {
                Line line = new Line();
                line.X1 = this.readingOrder.CoordinateTranslator.GetFromId(connector.origin.Item1).Output;
                line.X2 = this.readingOrder.CoordinateTranslator.GetFromId(connector.origin.Item2).Output;
                line.Y1 = this.readingOrder.CoordinateTranslator.GetFromId(connector.destination.Item1).Output;
                line.Y2 = this.readingOrder.CoordinateTranslator.GetFromId(connector.destination.Item2).Output;
                PlaceLine(line);
            }

            // Fill in the nodes
            foreach(Node node in this.readingOrder.Contents.Nodes)
            {
                Result<int> colNumber = this.readingOrder.CoordinateTranslator.GetFromId(node.GetX());
                Result<int> rowNumber = this.readingOrder.CoordinateTranslator.GetFromId(node.GetY());

                if(!(colNumber.Success && rowNumber.Success))
                {
                    Debug.WriteLine("Node has invalid coordinates");
                    continue;
                }
                Debug.WriteLine("Drawing node at " + rowNumber.Output + "," + colNumber.Output);
                PlaceNode(node, rowNumber.Output, colNumber.Output);
            }
        }

        private void PlaceLine(Line line)
        {
            line.Stroke = System.Windows.Media.Brushes.Black;
            line.SnapsToDevicePixels = true;
            line.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
            line.StrokeThickness = 10;
            ReadingOrderCanvas.Children.Add(line);
            Debug.WriteLine("Placed a line");
        }

        private void PlaceNode(Node node, int rowNumber, int columnNumber)
        {
            UIElement display = (UIElement)((DataTemplate)this.Resources["NodeDisplay"]).LoadContent();
            ((FrameworkElement)display).DataContext = node;

            int x = (columnNumber * IMAGE_SIZE) + (columnNumber * IMAGE_GAP_SIZE);
            int y = (rowNumber * IMAGE_SIZE) + (rowNumber * (IMAGE_GAP_SIZE/2));

            Canvas.SetLeft(display, x);
            Canvas.SetTop(display, y);

            ReadingOrderCanvas.Children.Add(display);
            Debug.WriteLine("Placed a node");
        }

        private void GetReadingOrder(Guid readingOrderId)
        {
            ReadingOrderOverview? overview = new ReadingOrderListProvider().GetReadingOrderOverview(readingOrderId);
            if(overview == null)
            {
                return;
            }

            ReadingOrder? result = new ReadingOrderProvider().GetReadingOrder(overview);

            if(result == null)
            {
                Debug.WriteLine("Failed to find reading order. Returning to previous page");
                ReadingOrderListPage navPage = new ReadingOrderListPage();
                NavigationService navigationService = NavigationService.GetNavigationService(this);
                navigationService.Navigate(navPage);
                navigationService.RemoveBackEntry();
                return;
            }
            Debug.WriteLine("Found reading order");
            this.readingOrder = result;
        }
    }
}
