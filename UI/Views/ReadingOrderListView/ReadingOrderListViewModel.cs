using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DynamicData;
using ROGraph.Backend.DataProviders.Interfaces;
using ROGraph.UI.ViewModels;
using ROGraph.Shared.Models;
using ROGraph.UI.Messages;

namespace ROGraph.UI.Views.ReadingOrderListView;

internal partial class ReadingOrderListViewModel : ObservableObject
{
    private readonly IReadingOrderProvider _readingOrderProvider;
    
    private ObservableCollection<ReadingOrderOverview> _overviews;

    public ObservableCollection<ReadingOrderOverview> Overviews
    {
        get => _overviews;
        set => SetProperty(ref _overviews, value);
    }

    public ReadingOrderListViewModel(IReadingOrderProvider roProvider)
    {
        _readingOrderProvider = roProvider;
        Overviews = new ObservableCollection<ReadingOrderOverview>(_readingOrderProvider.GetReadingOrders());
        RegisterMessages();
    }
    
    private void RefreshReadingOrders()
    {
        Overviews.Clear();
        
        _overviews.AddRange(_readingOrderProvider.GetReadingOrders());
    }

    private void RegisterMessages()
    {
        WeakReferenceMessenger.Default.Register<ReadingOrderAddedMessage>(this, (r,m) =>
        {
            _readingOrderProvider.CreateReadingOrder(m.Value);
            RefreshReadingOrders();
        });
    }
}