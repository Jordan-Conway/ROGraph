using ROGraph.Data;
using ROGraph.Models;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ROGraph
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : NavigationWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            ReadingOrders.Items.Add(0, new ReadingOrder() { Name = "RO1", PageNumber = 1 });
            ReadingOrders.Items.Add(1, new ReadingOrder() { Name = "RO2", PageNumber = 2 });
            //TODO: load list of reading orders from storage
        }
    }
}