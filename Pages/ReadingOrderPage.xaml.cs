using ROGraph.Components;
using ROGraph.Data;
using ROGraph.Data.DataProviders.Interfaces;
using ROGraph.Data.DataProviders.MockProviders;
using ROGraph.Data.DataProviders.SQLiteProviders;
using ROGraph.Enums;
using ROGraph.Models;
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
using static System.Net.Mime.MediaTypeNames;
using Image = System.Windows.Controls.Image;

namespace ROGraph.Pages
{
    /// <summary>
    /// Interaction logic for ReadingOrderPage.xaml
    /// </summary>
    /// 
    public partial class ReadingOrderPage : Page
    {
        private Guid readingOrderId;
        private ReadingOrder? readingOrder;

        private const int IMAGE_SIZE = 240;
        private const int IMAGE_GAP_SIZE = 80;

        private InteractionMode interactionMode = InteractionMode.DEFAULT;

        private (int, int) newConnectorOrigin;

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
                PlaceContent();
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            this.ReadingOrderCanvas.Children.Clear();
            this.PlaceContent();
        }

        private void PlaceContent()
        {
            if (this.readingOrder == null)
            {
                Debug.WriteLine("Reading Order is Null");
                return;
            }

            if (this.readingOrder.Contents == null)
            {
                Debug.WriteLine("Reading Order has no content");
                return;
            }

            if (this.readingOrder.CoordinateTranslator == null)
            {
                throw new NullReferenceException("Reading Order has no translator");
            }

            Debug.WriteLine("Connectors is Empty: " + (this.readingOrder.Contents.Connectors.Count == 0));
            Debug.WriteLine("Nodes is Empty: " + (this.readingOrder.Contents.Nodes.Count == 0));

            // Place in the connectors first
            foreach (Connector connector in this.readingOrder.Contents.Connectors)
            {
                PlaceConnector(connector);
            }

            // Fill in the nodes
            foreach (Node node in this.readingOrder.Contents.Nodes)
            {
                Result<int> colNumber = this.readingOrder.CoordinateTranslator.GetXFromId(node.GetX());
                Result<int> rowNumber = this.readingOrder.CoordinateTranslator.GetYFromId(node.GetY());

                if (!(colNumber.Success && rowNumber.Success))
                {
                    Debug.WriteLine("Node has invalid coordinates");
                    continue;
                }
                PlaceNode(node, rowNumber.Output, colNumber.Output);
            }
        }

        private void PlaceConnector(Connector connector)
        {
            if (this.readingOrder.CoordinateTranslator == null)
            {
                Debug.WriteLine("No translator present. Skipping drawing of connectors");
                return;
            }

            Line display = (Line)((DataTemplate)this.Resources["ConnectorDisplay"]).LoadContent();
            display.DataContext = connector;

            int x1 = this.readingOrder.CoordinateTranslator.GetXFromId(connector.origin.Item1).Output;
            int y1 = this.readingOrder.CoordinateTranslator.GetYFromId(connector.origin.Item2).Output;
            int x2 = this.readingOrder.CoordinateTranslator.GetXFromId(connector.destination.Item1).Output;
            int y2 = this.readingOrder.CoordinateTranslator.GetYFromId(connector.destination.Item2).Output;

            display.X1 = (x1 * IMAGE_SIZE) + (x1 * IMAGE_GAP_SIZE) + (IMAGE_SIZE / 2) + (IMAGE_GAP_SIZE / 2);
            display.Y1 = (y1 * IMAGE_SIZE) + (y1 * (IMAGE_GAP_SIZE / 2)) + (IMAGE_SIZE / 2) + (IMAGE_GAP_SIZE / 4);
            display.X2 = (x2 * IMAGE_SIZE) + (x2 * IMAGE_GAP_SIZE) + (IMAGE_SIZE / 2) + (IMAGE_GAP_SIZE / 2);
            display.Y2 = (y2 * IMAGE_SIZE) + (y2 * (IMAGE_GAP_SIZE / 2)) + (IMAGE_SIZE / 2) + (IMAGE_GAP_SIZE / 4);

            ReadingOrderCanvas.Children.Add(display);
        }

        private void PlaceNode(Node node, int rowNumber, int columnNumber)
        {
            StackPanel display = (StackPanel)((DataTemplate)this.Resources["NodeDisplay"]).LoadContent();
            display.DataContext = node;

            int x = (columnNumber * IMAGE_SIZE) + (columnNumber * IMAGE_GAP_SIZE) + (IMAGE_GAP_SIZE / 2);
            int y = (rowNumber * IMAGE_SIZE) + (rowNumber * (IMAGE_GAP_SIZE / 2)) + (IMAGE_GAP_SIZE / 4);

            Image nodeImage = VisualTreeHelper.GetChild(display, 0) as Image;
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(node.GetImagePath());
            image.EndInit();
            nodeImage.Source = image;

            Canvas.SetLeft(display, x);
            Canvas.SetTop(display, y);

            Debug.WriteLine($"Node is completed: {node.IsCompleted}");

            ReadingOrderCanvas.Children.Add(display);
        }

        private void GetReadingOrder(Guid readingOrderId)
        {
            IReadingOrderListProvider readingOrderListProvider;
            IReadingOrderProvider readingOrderProvider;

            if (Environment.GetEnvironmentVariable("USE_MOCK_PROVIDERS") == "true")
            {
                readingOrderListProvider = new MockReadingOrderListProvider();
                readingOrderProvider = new MockReadingOrderProvider();
            }
            else
            {
                readingOrderListProvider = new ReadingOrderListProvider();
                readingOrderProvider = new ReadingOrderProvider();
            }

            ReadingOrderOverview? overview = readingOrderListProvider.GetReadingOrderOverview(readingOrderId);

            if (overview == null)
            {
                return;
            }


            ReadingOrder? result = readingOrderProvider.GetReadingOrder(overview);

            if (result == null)
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

        private void OpenNodeContextMenu(object sender, MouseButtonEventArgs e)
        {
            Image image = sender as Image;
            image.ContextMenu.PlacementTarget = image;
            image.ContextMenu.IsOpen = true;
        }

        private void OpenConnectorContextMenu(object sender, MouseButtonEventArgs e)
        {
            ContextMenu menu = new ContextMenu();
            Line line = sender as Line;
            MenuItem deleteButton = new MenuItem();
            deleteButton.Header = "Delete";
            deleteButton.Click += (o, i) =>
            {
                ((Guid, Guid), (Guid, Guid)) c = (((Guid, Guid), (Guid, Guid)))line.Tag;
                DeleteConnector(c.Item1, c.Item2);
            };
            menu.Items.Add(deleteButton);
            ((FrameworkElement)sender).ContextMenu = menu;
            menu.IsOpen = true;
        }

        private void OpenEditNodeDialog(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;
            Guid id = (Guid)(item.Tag);
            Debug.WriteLine("Editing node " + id);

            Node? node = this.readingOrder.Contents.GetNode(id);

            if(node == null)
            {
                throw new NullReferenceException("Tried to edit node that is not present");
            }

            Window window = new Window
            {
                Title = "Edit Node",
                Content = new EditNodeDialog(node, this.UpdateNode)
            };

            window.Show();
        }

        private void OpenDeleteNodeDialog(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;
            Guid id = (Guid)(item.Tag);
            Debug.WriteLine("Deleting node " + id);

            Node? node = this.readingOrder.Contents.GetNode(id);

            if (node == null)
            {
                Debug.WriteLine("Tried to delete node that was not present");
                return;
            }

            Window window = new Window
            {
                Title = "Delete Node?",
                Content = new DeleteNodeDialog(node, this.DeleteNode)
            };

            window.Show();
        }
         
        private void UpdateNode(Node node)
        {
            this.readingOrder.Contents.ReplaceNode(node);
            this.InvalidateVisual();
        }

        private void DeleteNode(Node node)
        {
            this.readingOrder.Contents.DeleteNode(node);
            this.InvalidateVisual();
        }

        private void DeleteConnector((Guid, Guid) origin, (Guid, Guid) destination)
        {
            Debug.WriteLine($"Deleting connector between {origin} and {destination}");
            bool isDeleted = this.readingOrder.Contents.deleteConnector(origin, destination);
            Debug.WriteLine($"Connector was deleted: {isDeleted}");
            this.InvalidateVisual();
        }

        private void AddNewNode(object sender, RoutedEventArgs e)
        {
            (int, int) position = this.GetCoordinatesFromMousePosition();

            Guid x = this.readingOrder.CoordinateTranslator.GetXFromInt(position.Item1);
            Guid y = this.readingOrder.CoordinateTranslator.GetYFromInt(position.Item2);

            // TODO: Show this to the user
            if (this.readingOrder.Contents.NodeExistsAtPosition(x, y))
            {
                Debug.WriteLine("Only one node can exist at a given position");
                return;
            }

            Node node = new DrawableNode(
                Guid.NewGuid(),
                "New Node",
                this.readingOrder.Id,
                DateTime.Now,
                DateTime.Now,
                x,
                y,
                Enums.NodeType.Triangle,
                false);

            this.readingOrder.Contents.Nodes.Add(node);
            this.InvalidateVisual();
        }

        private void AddNewConnector(object sender, RoutedEventArgs e)
        {
            this.newConnectorOrigin = this.GetCoordinatesFromMousePosition();
            this.interactionMode = InteractionMode.NEW_CONNECTOR;
        }

        private void OpenDefaultContextMenu(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("Opening Context Menu");
            Canvas canvas = sender as Canvas;

            canvas.ContextMenu.PlacementTarget = canvas;
            canvas.ContextMenu.IsOpen = true;
        }

        private void AddNewRow(object sender, RoutedEventArgs e)
        {
            (int, int) position = this.GetCoordinatesFromMousePosition();

            this.readingOrder.AddRow(position.Item2);

            this.InvalidateVisual();
        }

        private void AddNewColumn(object sender, RoutedEventArgs e)
        {
            (int, int) position = this.GetCoordinatesFromMousePosition();

            this.readingOrder.AddColumn(position.Item1);

            this.InvalidateVisual();
        }

        private (int, int) GetCoordinatesFromMousePosition()
        {
            Point position = Mouse.GetPosition(this.ReadingOrderCanvas);

            int colPosition = 0;
            int rowPosition = 0;

            int nodeWidth = IMAGE_SIZE + IMAGE_GAP_SIZE;
            int nodeHeight = IMAGE_SIZE + (IMAGE_GAP_SIZE / 2);

            for (int i = (int)position.X; i > nodeWidth; i -= nodeWidth)
            {
                colPosition++;
            }

            for (int i = (int)position.Y; i > nodeHeight; i -= nodeHeight)
            {
                rowPosition++;
            }

            return (colPosition, rowPosition);
        }

        private void PlaceConnectorDestination(object sender, MouseButtonEventArgs e)
        {
            if(this.interactionMode != InteractionMode.NEW_CONNECTOR)
            {
                return;
            }

            (int, int) destination = GetCoordinatesFromMousePosition();

            if (this.newConnectorOrigin == destination)
            {
                return;
            }

            this.readingOrder.AddConnector(this.newConnectorOrigin, destination);

            this.interactionMode = InteractionMode.DEFAULT;

            this.InvalidateVisual();
        }
    }
}
