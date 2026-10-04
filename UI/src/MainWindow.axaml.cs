using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using ROGraph.Backend.Contracts;

namespace ROGraph.UI;

public partial class MainWindow : Window
{
    public MainWindow(MainWindowViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}