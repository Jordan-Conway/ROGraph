using Avalonia.Controls;
using ROGraph.Backend.Contracts;

namespace ROGraph.UI;

public partial class MainWindow : Window
{
    public MainWindow(IReadingOrderProvider readingOrderProvider)
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel(readingOrderProvider);
    }
}