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
using System.Xml.Linq;

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
            ReadingOrder ro1 = new ReadingOrder("RO1", 1, new ReadingOrderNodes());
            ReadingOrder ro2 = new ReadingOrder("RO2", 2, new ReadingOrderNodes());

            ReadingOrders.Items.Add(0, ro1);
            ReadingOrders.Items.Add(1, ro2);
            //TODO: load list of reading orders from storage
        }
    }
}