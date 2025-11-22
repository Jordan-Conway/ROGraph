using Avalonia.Controls;
using ROGraph.Shared.Models;

namespace ROGraph.NewUI.Views.ReadingOrderView;

internal partial class ReadingOrderViewControl : UserControl
{

    public ReadingOrderViewControl(ReadingOrder readingOrder)
    {
        InitializeComponent();

        DataContext = new ReadingOrderViewModel(readingOrder);
    }
}