using System.Collections.Generic;
using ROGraph.Backend.DataProviders.Interfaces;
using ROGraph.UI.ViewModels;
using ROGraph.Shared.Models;

namespace ROGraph.UI.Views.ReadingOrderListView;

internal partial class ReadingOrderListViewModel : ViewModelBase
{
    public IReadingOrderListProvider ListProvider { get; set; }
    private readonly IReadingOrderProvider _roProvider;

    public List<ReadingOrderOverview> Overviews { get; set; }= [];

    public ReadingOrderListViewModel(IReadingOrderListProvider listProvider, IReadingOrderProvider roProvider)
    {
        this.ListProvider = listProvider;
        this._roProvider = roProvider;
        this.Overviews = listProvider.GetReadingOrders();
    }
}