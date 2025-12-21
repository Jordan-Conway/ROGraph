using System;
using System.Collections.Generic;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using ROGraph.Backend.Data.DataProviders.Interfaces;
using ROGraph.UI.ViewModels;
using ROGraph.UI.Views.ReadingOrderView;
using ROGraph.Shared.Models;

namespace ROGraph.UI.Views.ReadingOrderListView;

internal partial class ReadingOrderListViewModel : ViewModelBase
{
    public IReadingOrderListProvider ListProvider { get; set; }
    private readonly IReadingOrderProvider roProvider;

    public List<ReadingOrderOverview> Overviews { get; set; }= [];

    public ReadingOrderListViewModel(IReadingOrderListProvider listProvider, IReadingOrderProvider roProvider)
    {
        this.ListProvider = listProvider;
        this.roProvider = roProvider;
        this.Overviews = listProvider.GetReadingOrders();
    }
}