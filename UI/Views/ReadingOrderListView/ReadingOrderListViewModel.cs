using System.Collections.Generic;
using ROGraph.Backend.DataProviders.Interfaces;
using ROGraph.UI.ViewModels;
using ROGraph.Shared.Models;

namespace ROGraph.UI.Views.ReadingOrderListView;

internal partial class ReadingOrderListViewModel : ViewModelBase
{
    private readonly IReadingOrderProvider _roProvider;

    public List<ReadingOrderOverview> Overviews { get; set; }= [];

    public ReadingOrderListViewModel(IReadingOrderProvider roProvider)
    {
        this._roProvider = roProvider;
        this.Overviews = roProvider.GetReadingOrders();
    }
}