using ROGraph.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace ROGraph.Components
{
    /// <summary>
    /// Interaction logic for EditNodeDialog.xaml
    /// </summary>
    public partial class EditNodeDialog : UserControl
    {
        public Node node;
        public Action<Node> updateNodeValue;

        public EditNodeDialog(Node node, Action<Node> updateNodeValue)
        {
            InitializeComponent();
            this.DataContext = node;

            this.node = node;
            this.updateNodeValue = updateNodeValue;
        }

        private void UpdateName(object sender, TextChangedEventArgs e)
        {
            string? newName = (sender as TextBox).Text;

            if (newName == null)
            {
                this.node.Name = string.Empty;
            }
            else
            {
                this.node.Name = newName;
            }
        }

        private void UpdateDescription(object sender, TextChangedEventArgs e)
        {
            string? newDesc = (sender as TextBox).Text;

            if (newDesc == null)
            {
                this.node.Description = string.Empty;
            }
            else
            {
                this.node.Description = newDesc;
            }
        }

        private void UpdateCompleted(object sender, RoutedEventArgs e)
        {
            this.node.IsCompleted = true;
        }

        private void UpdateNotCompleted(object sender, RoutedEventArgs e)
        {
            this.node.IsCompleted = false;
        }

        private void ApplyEdits(object sender, RoutedEventArgs e)
        {
            this.updateNodeValue(this.node);
            Window.GetWindow(this).Close();
        }
    }
}
