using Avalonia.Controls;
using ROGraph.Backend.DataProviders.Interfaces;

namespace ROGraph.UI;

public partial class MainWindow : Window
{
    public MainWindow(IReadingOrderProvider readingOrderProvider)
    {
        InitializeComponent();
        this.DataContext = new MainWindowViewModel(readingOrderProvider);
    }
}