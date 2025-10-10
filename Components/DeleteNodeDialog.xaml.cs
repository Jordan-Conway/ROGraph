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

namespace ROGraph.Components
{
    /// <summary>
    /// Interaction logic for DeleteNodeDialog.xaml
    /// </summary>
    public partial class DeleteNodeDialog : UserControl
    {
        private Node node;
        private Action<Node> deleteNode;
        public DeleteNodeDialog(Node node, Action<Node> deleteNode)
        {
            InitializeComponent();
            this.node = node;
            this.deleteNode = deleteNode;
        }

        private void CloseDialog(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close(); 
        }

        private void DeleteNode(object sender, RoutedEventArgs e)
        {
            this.deleteNode(this.node);
            this.CloseDialog(sender, e);
        }
    }
}
