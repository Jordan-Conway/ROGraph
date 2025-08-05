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

        private const int STARTING_TOP_OFFSET = 100;
        private const int STARTING_LEFT_OFFSET = 400;
        public ReadingOrderPage(ReadingOrder readingOrder)
        {
            this.readingOrder = readingOrder;
            InitializeComponent();
            PopulateNodes();
        }

        private void PopulateNodes()
        {
            //TODO: Change to use actual data
            // Place the root node
            Node root = new Node { Name = "My Reading Order", Type = Enums.NodeType.Diamond };
            PlaceNode(root, 0, 0);

            root.Children.Add(new Node { Name = "My Reading Order", Type = Enums.NodeType.Diamond });
            root.Children.Add(new Node { Name = "My Reading Order", Type = Enums.NodeType.Diamond });

            PlaceChildNodes(root, STARTING_LEFT_OFFSET, STARTING_TOP_OFFSET);
        }

        private void PlaceNode(Node node, int leftOffset, int topOffset)
        {
            UIElement display = (UIElement)((DataTemplate)this.Resources["NodeDisplay"]).LoadContent();
            ((FrameworkElement)display).DataContext = node;

            Canvas.SetTop(display, 100 + topOffset);
            Canvas.SetLeft(display, 20 + leftOffset);

            ReadingOrderCanvas.Children.Add(display);
        }

        private void PlaceChildNodes(Node root, int leftOffset, int topOffset)
        {
            if(!root.Children.Any())
            {
                return;
            }
            int shiftDown = 0;
            foreach (Node child in root.Children)
            {
                PlaceNode(child, leftOffset, topOffset + (shiftDown*400));
                shiftDown++;
                PlaceChildNodes(child, leftOffset * 2, topOffset);
            }
        }
    }
}
