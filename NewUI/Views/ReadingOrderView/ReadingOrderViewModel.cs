using ROGraph.NewUI.ViewModels;
using ROGraph.Shared.Models;

namespace ROGraph.NewUI.Views.ReadingOrderView;

internal partial class ReadingOrderViewModel : ViewModelBase
{
    public ReadingOrder ReadingOrder { get; set; }

    public ReadingOrderViewModel(ReadingOrder readingOrder)
    {
        this.ReadingOrder = readingOrder;
    }
}